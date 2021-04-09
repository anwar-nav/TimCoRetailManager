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
    /// This is used for posting sale to API
    /// </summary>
    public class SaleEndpoint : ISaleEndpoint
    {
        //Declared private property
        private IAPIHelper _apiHelper;

        //This constructor is pulling in IAPIHelper and storing in _apiHelper for the life span of this class instance.
        public SaleEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        //This will get product data from API endpoint
        public async Task PostSale(SaleModel sale)
        {
            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be mapped with _loggedInUser for mapping the object. Can be done
            //by automapper tool but here we are manually mapping it.
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                //We are not adding the token in header because the instance of ApiClient is same which was used to get
                //user info and at that token was added to the header.
                if (response.IsSuccessStatusCode)
                {
                    // Log Sucessfull call
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
