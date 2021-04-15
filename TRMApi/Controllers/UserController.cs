using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TRMApi.Data;
using TRMApi.Models;
using TRMDataManger.Library.DataAccess;
using TRMDataManger.Library.Models;

namespace TRMApi.Controllers
{
    /// <summary>
    /// User endpoint in API to process operations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, 
                                UserManager<IdentityUser> userManager, 
                                IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        //This will create UserModel and will get userId from Entity Framework Table and use that
        //Id in stored procedure to search user in User Table and get all the data of User.
        [HttpGet]
        public UserModel GetById()
        {
            //This will get user id from entity framework user table using Entity Framework.
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString(); //.Net Framework way - RequestContext.Principal.Identity.GetUserId();
            //This will create an instance of UserData class from Class Library.
            UserData data = new UserData(_config);
            //This will return the data.
            return data.GetUserById(userId).First();
        }

        //This method will return a List of ApplicationUserModel having the details of Users present in database with values
        //of User Id , Email and roles (with roleid and role name).
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            //This is object output of List of ApplicationUserModel which will be returned at end.
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            //This is storing all the users present in database(AspNetUsers table) using the _context created in constructor.
            var users = _context.Users.ToList();

            //This is storing all the UserId, RolesId and Role Name present in database using the  _context created in 
            //constructor. LINQ statement is used here.
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

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

                //This is storing RoleId and Role Name specifically added to a certain user.
                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

                //Added the populated ApplicationUserModel to List of ApplicationUserModel.
                output.Add(u);
            }


            return output;
        }

        //This method returns all the Roles present in database using Entity Framework.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            //This is storing all the roles present in database(AspNetRoles table) using the Db connection _context.
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        //This method adds a Role in database using Entity Framework and takes the parameter of UserRolePairModel instead of
        //directly passing in the userid and rolename for security.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/AddRole")]
        public async Task AddARole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            //This adds a role to user.
            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        //This method adds a Role in database using Entity Framework and takes the parameter of UserRolePairModel instead of
        //directly passing in the userid and rolename for security.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/RemoveRole")]
        public async Task RemoveARole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            //This adds a role to user.
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }

    }
}
