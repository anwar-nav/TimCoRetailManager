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
    /// This class will be used to call data from database and store the return values in ProductModel.
    /// </summary>
    public class ProductData : IProductData
    {
        private readonly ISQLDataAccess _sql;

        public ProductData(ISQLDataAccess sql)
        {
            _sql = sql;
        }

        //For usage purposes an anonymous object of type dynamic is passed as the second argument of
        //LoadData method. This usage of dynamic only works if it is in same assembly.
        public List<ProductModel> GetProducts()
        {
            //Beneficial for Unit testing.
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");

            return output;
        }

        //This method will return the product details by id.
        public ProductModel GetProductsById(int productId)
        {
            //Beneficial for Unit testing.
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { id = productId }, "TRMData").FirstOrDefault();

            return output;
        }

    }
}
