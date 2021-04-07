using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
    /// <summary>
    /// This is an Interface of APIHelper class.
    /// </summary>
    public interface IAPIHelper
    {
        Task<AuthenticateUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}