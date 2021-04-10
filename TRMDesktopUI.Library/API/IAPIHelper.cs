using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
    /// <summary>
    /// This is an Interface of APIHelper class.
    /// </summary>
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticateUser> Authenticate(string username, string password);
        void LogOfUser();
        Task GetLoggedInUserInfo(string token);
    }
}