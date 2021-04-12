using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRMApi.Models
{
    /// <summary>
    /// This will be used for storing the values of UserId and RoleName to pass to UserController add and remove methods.
    /// </summary>
    public class UserRolePairModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
