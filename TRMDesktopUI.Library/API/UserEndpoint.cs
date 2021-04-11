using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
    /// <summary>
    /// This is used for interaction with API endpoint related to User.
    /// </summary>
    public class UserEndpoint : IUserEndpoint
    {
        //Declared private property
        private readonly IAPIHelper _apiHelper;

        //This constructor is pulling in IAPIHelper and storing in _apiHelper for the life span of this class instance.
        public UserEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        //This will get all the users from API endpoint.
        public async Task<List<UserModel>> GetAll()
        {
            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be create a list of UserModel and then it will be the result object.
            //result will be returned.
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/User/Admin/GetAllUsers"))
            {
                //We are not adding the token in header because the instance of ApiClient is same which was used to get
                //user info and at that token was added to the header.
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //This will get all the Roles available for users from API endpoint.
        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be create a dictionary of string key and string value and then it 
            //will be the result object.
            //result will be returned.
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/User/Admin/GetAllRoles"))
            {
                //We are not adding the token in header because the instance of ApiClient is same which was used to get
                //user info and at that token was added to the header.
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //This will add a Role to user from API endpoint.
        public async Task AddUserToRole(string userId, string roleName)
        {
            //To hold values received in parameter.
            var data = new { userId = userId, roleName = roleName };

            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be create a dictionary of string key and string value and then it 
            //will be the result object.
            //result will be returned.
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/User/Admin/AddRole", data))
            {
                //We are not adding the token in header because the instance of ApiClient is same which was used to get
                //user info and at that token was added to the header.
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //This will remove a Role from user from API endpoint.
        public async Task RemoveUserFromRole(string userId, string roleName)
        {
            //To hold values received in parameter.
            var data = new { userId = userId, roleName = roleName };

            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be create a dictionary of string key and string value and then it 
            //will be the result object.
            //result will be returned.
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/User/Admin/RemoveRole", data))
            {
                //We are not adding the token in header because the instance of ApiClient is same which was used to get
                //user info and at that token was added to the header.
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
