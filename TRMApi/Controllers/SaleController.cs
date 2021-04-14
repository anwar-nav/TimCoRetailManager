using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Models;

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
        private readonly IConfiguration _config;

        public SaleController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// This posts the sale to database by using the method specified in class library.
        /// </summary>
        /// <param name="sale">This will hold the details of sales provided by UI in form of SaleModel.</param>
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            //Instantiating this class in order to access the savesale method.
            SaleData data = new SaleData(_config);
            //This will get user id from entity framework user table.
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).ToString(); //.Net Framework way - RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
        }

        /// <summary>
        /// This gets the sales report by using the method specified in class library.
        /// </summary>
        /// <returns></returns>
        [Route("GetSalesReport")]
        [HttpGet]
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


            SaleData data = new SaleData(_config);
            return data.GetSaleReport();
        }

    }
}
