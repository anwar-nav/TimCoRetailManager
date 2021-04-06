using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Models
{
    /// <summary>
    /// This will hold the values received from API in response content.
    /// </summary>
    public class AuthenticateUser
    {
        public string Access_Token { get; set; }
        public string UserName { get; set; }
    }
}
