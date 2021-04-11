using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// This will be used to store data received from API.
    /// </summary>
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        //This property will store the roles received from API
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
        public string RoleList
        {
            get
            {
                return string.Join(", ", Roles.Select(x => x.Value));    
            }
        }
    }
}
