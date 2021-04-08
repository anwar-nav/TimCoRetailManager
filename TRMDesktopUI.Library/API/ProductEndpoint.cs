using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
    public class ProductEndpoint : IProductEndpoint
    {
        //Declared private property
        private IAPIHelper _apiHelper;

        //This constructor is pulling in IAPIHelper and storing in _apiHelper for the life span of this class instance.
        public ProductEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        //This will get product data from API endpoint
        public async Task<List<ProductModel>> GetAll()
        {
            //Creating a response message to receive data from API by using a GetAsync method in which the route(Uri)
            //and data being sent is given.
            //response received has content which will be mapped with _loggedInUser for mapping the object. Can be done
            //by automapper tool but here we are manually mapping it.
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
            {
                //We are not adding the token in header because the instance of ApiClient is same which was used to get
                //user info and at that token was added to the header.
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
