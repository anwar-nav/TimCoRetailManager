using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManger.Library.Internal.DataAccess;
using TRMDataManger.Library.Models;

namespace TRMDataManger.Library.DataAccess
{
    /// <summary>
    /// This class will be used to call data from database and store the return values in List<InventoryModel> and
    /// save inventory record in database.
    /// </summary>
    public class InventoryData
    {
        //This call the spInventory_GetAll and return the data.
        public List<InventoryModel> GetInventory()
        {
            SQLDataAccess sql = new SQLDataAccess();

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TRMData");

            return output;
        }

        //This will save inventory record in database.
        public void SaveInventoryRecord(InventoryModel item)
        {
            SQLDataAccess sql = new SQLDataAccess();

            sql.SaveData("dbo.spInventory_Insert", item, "TRMData");
        }
    }
}
