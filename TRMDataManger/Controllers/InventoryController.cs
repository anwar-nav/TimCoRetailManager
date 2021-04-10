using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Models;

namespace TRMDataManger.Controllers
{
    /// <summary>
    /// Inventory Endpoint in API to process operations.
    /// </summary>
    [Authorize]
    public class InventoryController : ApiController
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

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}