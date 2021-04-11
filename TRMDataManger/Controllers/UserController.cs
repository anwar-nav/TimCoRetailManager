using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Models;
using TRMDataManger.Models;

namespace TRMDataManger.Controllers
{
    /// <summary>
    /// User endpoint in API to process operations.
    /// </summary>
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
        [HttpGet]
        public UserModel GetById()
        {
            //This will get user id from entity framework user table using Entity Framework.
            string userId = RequestContext.Principal.Identity.GetUserId();
            //This will create an instance of UserData class from Class Library.
            UserData data = new UserData();
            //This will return the data.
            return data.GetUserById(userId).First();
        }

        //This method will return a List of ApplicationUserModel having the details of Users present in database with values
        //of User Id , Email and roles (with roleid and role name).
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            //This is object output of List of ApplicationUserModel which will be returned at end.
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            //This is using ApplicationDbContext which is connection made with database using EntityFramework.
            using (var context = new ApplicationDbContext())
            {
                //This creating a userstore based on Db connection context which is in using statement.
                var userStore = new UserStore<ApplicationUser>(context);
                //This is creating usermanager based on userstore created above.
                var userManager = new UserManager<ApplicationUser>(userStore);

                //This is storing all the users present in database(AspNetUsers table) using the usermanager created above.
                var users = userManager.Users.ToList();

                //This is storing all the roles present in database(AspNetRoles table) using the Db connection 
                //context which is in using statement.
                var roles = context.Roles.ToList();

                //can use LINQ
                //This will go through all the users present in users variable and for each user a new 
                //ApplicationUserModel(which will hold the details required to send to UI) will be created and populated.
                foreach (var user in users)
                {
                    //creating ApplicationUserModel and setting the id and email values from user variable.
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };

                    //This will go through all the roles present in user and every role will be added to
                    //ApplicationUserModel object(created above) in format of roleid and then name of the role.
                    foreach (var r in user.Roles)
                    {
                        //x.Id is the Id present in AspNetRoles table
                        //r.RoleId is the roleid present in AspNetUserRoles table. this is same in user object roles.
                        //comparison is done between x.Id and r.RoleId and when match found then respective name is added
                        //from AspNetRoles table and in memory roles object created above has these values.
                        u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
                    }

                    //Added the populated ApplicationUserModel to List of ApplicationUserModel.
                    output.Add(u);
                }

            }

            return output;
        }

        //This method returns all the Roles present in database using Entity Framework.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            //This is using ApplicationDbContext which is connection made with database using EntityFramework.
            using (var context = new ApplicationDbContext())
            {
                //This is storing all the roles present in database(AspNetRoles table) using the Db connection 
                //context which is in using statement.
                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
            }
        }

        //This method adds a Role in database using Entity Framework and takes the parameter of UserRolePairModel instead of
        //directly passing in the userid and rolename for security.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public void AddARole(UserRolePairModel pairing)
        {
            //This is using ApplicationDbContext which is connection made with database using EntityFramework.
            using (var context = new ApplicationDbContext())
            {
                //This creating a userstore based on Db connection context which is in using statement.
                var userStore = new UserStore<ApplicationUser>(context);
                //This is creating usermanager based on userstore created above.
                var userManager = new UserManager<ApplicationUser>(userStore);
                //This adds a role to user.
                userManager.AddToRole(pairing.UserId, pairing.RoleName);
            }
        }

        //This method adds a Role in database using Entity Framework and takes the parameter of UserRolePairModel instead of
        //directly passing in the userid and rolename for security.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public void RemoveARole(UserRolePairModel pairing)
        {
            //This is using ApplicationDbContext which is connection made with database using EntityFramework.
            using (var context = new ApplicationDbContext())
            {
                //This creating a userstore based on Db connection context which is in using statement.
                var userStore = new UserStore<ApplicationUser>(context);
                //This is creating usermanager based on userstore created above.
                var userManager = new UserManager<ApplicationUser>(userStore);
                //This removes the role from user.
                userManager.RemoveFromRole(pairing.UserId, pairing.RoleName);
            }
        }
    }
}