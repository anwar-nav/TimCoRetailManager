using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRMApi.Models
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        //This property will store the roles received from API library data access.
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
    }
}
