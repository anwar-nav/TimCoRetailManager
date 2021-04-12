using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRMApi.Controllers
{
    /// <summary>
    /// Sale Endpoint in API to process operations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        /// <summary>
        /// This posts the sale to database by using the method specified in class library.
        /// </summary>
        /// <param name="sale">This will hold the details of sales provided by UI in form of SaleModel.</param>
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            //Instantiating this class in order to access the savesale method.
            SaleData data = new SaleData();
            //This will get user id from entity framework user table.
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).ToString(); //.Net Framework way - RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
        }

        /// <summary>
        /// This gets the sales report by using the method specified in class library.
        /// </summary>
        /// <returns></returns>
        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin, Manager")]
        public List<SaleReportModel> GetSalesReport()
        {
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    // Do admin stuff
            //}
            //else if (RequestContext.Principal.IsInRole("Manager"))
            //{
            //    // Do manager stuff
            //}


            SaleData data = new SaleData();
            return data.GetSaleReport();
        }


        //// GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
