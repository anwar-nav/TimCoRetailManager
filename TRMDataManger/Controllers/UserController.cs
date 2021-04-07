using Microsoft.AspNet.Identity;
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
    [Authorize]
    public class UserController : ApiController
    {
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

        //This will create UserModel and will get userId from Entity Framework Table and use that
        //Id in stored procedure to search user in User Table and get all the data of User.
        public UserModel GetById()
        {
            //This will get user id from entity framework user table.
            string userId = RequestContext.Principal.Identity.GetUserId();
            //This will create an instance of UserData class from Class Library.
            UserData data = new UserData();
            //This will return the data.
            return data.GetUserById(userId).First();
        }
    }
}