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
    /// This class will be used to call data from database and store the return values in List<InventoryModel> and
    /// save inventory record in database.
    /// </summary>
    public class InventoryData : IInventoryData
    {
        private readonly ISQLDataAccess _sql;

        public InventoryData(ISQLDataAccess sql)
        {
            _sql = sql;
        }

        //This call the spInventory_GetAll and return the data.
        public List<InventoryModel> GetInventory()
        {
            var output = _sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TRMData");

            return output;
        }

        //This will save inventory record in database.
        public void SaveInventoryRecord(InventoryModel item)
        {
            _sql.SaveData("dbo.spInventory_Insert", item, "TRMData");
        }
    }
}
