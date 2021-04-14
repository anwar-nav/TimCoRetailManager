using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Models;


namespace TRMApi.Controllers
{
    /// <summary>
    /// Inventory Endpoint in API to process operations.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// This gets the inventory data by using the method specified in class library.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData();
            return data.GetInventory();
        }

        /// <summary>
        /// This posts the inventory data by using the method specified in class library.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Manager")]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(item);
        }
    }
}
