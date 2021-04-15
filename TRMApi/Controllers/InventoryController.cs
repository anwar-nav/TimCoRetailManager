using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IInventoryData _inventoryData;

        public InventoryController(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        /// <summary>
        /// This gets the inventory data by using the method specified in class library.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public List<InventoryModel> Get()
        {
            return _inventoryData.GetInventory();
        }

        /// <summary>
        /// This posts the inventory data by using the method specified in class library.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Manager")]
        public void Post(InventoryModel item)
        {
            _inventoryData.SaveInventoryRecord(item);
        }
    }
}
