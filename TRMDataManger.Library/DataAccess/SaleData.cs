using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TRMDataManger.Library.Internal.DataAccess;
using TRMDataManger.Library.Models;

namespace TRMDataManger.Library.DataAccess
{
    /// <summary>
    /// This class will be used to store sale data into database and will be called in API.
    /// </summary>
    public class SaleData
    {
        private readonly IConfiguration _config;

        public SaleData(IConfiguration config)
        {
            _config = config;
        }

        //This method is used to store sale and sale detail data into database.
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData(_config); // for getting the productId.
            var taxRate = ConfigHelper.GetTaxRate()/100; //for getting taxrate from API web.config.


            //A loop which goes throug every item in SaleModel provided by UI.
            //For every item the values provided by UI is stored in variable named detail and further values of this
            //variable are populated by getting the product information from database and performing calculation on this
            //information after validating the product from database and then this detail variable holding values of each
            //item is added to the list.
            foreach (var item in saleInfo.SaleDetails)
            {
                //creating a variable of type SaleDetailDBModel and storing the values passed from UI salemodel which
                //is a list of UI SaleDetailModel into a variable named detail of type SaleDetailDBModel.
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Validating product existence in database and adding the details in productinfo variable.
                var productinfo = products.GetProductsById(item.ProductId);

                if (productinfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }

                //storing the purchase price by calculation done on retail price present in product info variable which
                //was used to get the product details from database and detail quantity which was populated from the
                //sale model received from UI.
                detail.PurchasePrice = (productinfo.RetailPrice * detail.Quantity);

                //Checking that product in productinfo has taxable product and if it is then calculate the tax
                //by purchase price calculated in previous step multiplied by tax rate got from API web.config and store it
                //in detail.
                if (productinfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                //Store the detail variable in details list which is type of SaleDetailDBModel.
                details.Add(detail);
            }

            //A new object of SaleDBModel is created which stores the cumlative values of the whole sale from the list
            //populated above. The cashierid is set to the value provided the caller of this method. As API is calling
            //this method so API will get the cashierId by looking up the database since this API can only be accessed
            //by authorized.
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            //This insertion is by way of transaction.
            using (SQLDataAccess sql = new SQLDataAccess(_config))
            {
                try
                {
                    //This starts the transaction.
                    sql.StartTransaction("TRMData");

                    //This calls the savedata method in this library and gives the name of store procedure to use along with
                    //populated sale data.
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    //This gets the sales id of the sales just added in database. This is required because in saledetail the
                    //saleid is foreign key to the id present in sale table
                    int saleid = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                    //This adds all items of list populated above of sales items to the saledetail table based on sale id.
                    //For multiple calls to database look into advance dapper.
                    foreach (var item in details)
                    {
                        item.SaleId = saleid;
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    //This commits the transaction.
                    sql.CommitTransaction();
                }
                catch
                {
                    //This rolls back the transaction if any of the insert is unsuccessfull.
                    sql.RollbackTransaction();
                    throw;
                }
            }

            ////This was the individual way inserting sale.
            ////This calls the savedata method in this library and gives the name of store procedure to use along with
            ////populated sale data and name of database.
            //SQLDataAccess sql = new SQLDataAccess();
            //sql.SaveData("dbo.spSale_Insert", sale, "TRMData");

            ////This gets the sales id of the sales just added in database. This is required because in saledetail the
            ////saleid is foreign key to the id present in sale table
            //int saleid = sql.LoadData<int, dynamic>("dbo.spSale_Lookup", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }, "TRMData").FirstOrDefault();

            ////This adds all items of list populated above of sales items to the saledetail table based on sale id.
            ////For multiple calls to database look into advance dapper.
            //foreach (var item in details)
            //{
            //    item.SaleId = saleid;
            //    sql.SaveData("dbo.spSaleDetail_Insert", item, "TRMData");
            //}
        }

        //This call the spSale_SaleReport and return the data.
        public List<SaleReportModel> GetSaleReport()
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");

            return output;
        }
    }
}
