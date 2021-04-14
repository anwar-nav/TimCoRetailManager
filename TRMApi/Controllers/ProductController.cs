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
    /// Product endpoint in API to process operations.
    /// </summary>
    [Authorize(Roles = "Cashier")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET api/<controller>
        //This will create a List of ProductModel and use class library to access data and get products.
        [HttpGet]
        public List<ProductModel> Get()
        {
            //This will get user id from entity framework user table.
            //string userId = RequestContext.Principal.Identity.GetUserId();
            //This will create an instance of ProductData class from Class Library.
            ProductData data = new ProductData();
            //This will return the data.
            return data.GetProducts();
        }
    }
}
