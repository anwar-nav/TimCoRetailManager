using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;


namespace TRMDesktopUI.Library.API
{
    /// <summary>
    /// This will inherit IAPIHelper interface.
    /// Create an HTTP client that will send request to API
    /// Receive a response and create a model of authenticated user
    /// </summary>
    public class APIHelper : IAPIHelper
    {
        //Declaring a private object ApiClient of type HTTPClient
        private HttpClient apiClient { get; set; }
        //Declaring a private object LoggedInUserModel of type ILoggedInUserModel
        private ILoggedInUserModel _loggedInUser { get; set; }

        //This constructor will call the InitializeClient method and ask for the instance of ILoggedInUserModel and then
        //it will set the private object of _loggedInUser (Dependency injection).
        public APIHelper(ILoggedInUserModel loggedIn)
        {
            InitializeClient();
            _loggedInUser = loggedIn;
        }

        //This will grab the API route and store in string variable.
        //Instantiate apiClient and configures it with Uri and Header value type i.e. in which format the response body should be.
        private void InitializeClient()
        {
            //Getting API route string from App.config and storing it in string variable.
            string apiRoute = ConfigurationManager.AppSettings["apiRoute"];

            apiClient = new HttpClient(); //Instantiating a new HTTP client.
            apiClient.BaseAddress = new Uri(apiRoute); //Setting the Base Address of instantiated client.
            apiClient.DefaultRequestHeaders.Accept.Clear(); //Clearing the Headers to be blank for fresh start.
            //Adding Header value type to be json. it can be xml also.This will tell the response body should be in this format.
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //This method will send data asynchronously to api endpoint and in response will receive data
        //which will be transformed into a model using deserialization.
        public async Task<AuthenticateUser> Authenticate(string username, string password)
        {
            //Creating a variable and storing FormUrlEncodedContent with an array having values in Key Value pair format.
            //This is required because API requires data in this format for generation of token.
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            //Creating a response message to receive data from API by using a PostAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be stored in a variable named result.
            using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticateUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //This method will get all the info of User Logged in and store it in LoggedInUserModel.
        public async Task GetLoggedInUserInfo(string token)
        {
            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be mapped with _loggedInUser for mapping the object. Can be done
            //by automapper tool but here we are manually mapping it.
            using (HttpResponseMessage response = await apiClient.GetAsync("/api/User"))
            {
                apiClient.DefaultRequestHeaders.Clear(); //Clearing the Headers to be blank for fresh start.
                apiClient.DefaultRequestHeaders.Accept.Clear(); //Clearing the Headers to be blank for fresh start.
                //Adding Header value type to be json. it can be xml also.This will tell the response body should be in this format.
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //This creates a header with Authorization type and adds value in string format of Bearer token where token
                //is the value received from AuthenticateUser class.
                apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer { token }");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.Token = token;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.CreatedDate = result.CreatedDate;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
