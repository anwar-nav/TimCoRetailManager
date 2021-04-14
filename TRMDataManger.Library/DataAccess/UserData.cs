using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TRMDataManger.Library.Internal.DataAccess;
using TRMDataManger.Library.Models;

namespace TRMDataManger.Library.DataAccess
{
    /// <summary>
    /// This class will be used to call data from database and store the return values in UserModel.
    /// </summary>
    public class UserData
    {
        private readonly IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }

        //This method create an instance for SQLDataAccess class and call the LoadData method of it.
        //For usage purposes an anonymous object of type dynamic is passed as the second argument of
        //LoadData method. This usage of dynamic only works if it is in same assembly.
        public List<UserModel> GetUserById(string Id)
        {
            SQLDataAccess sql = new SQLDataAccess(_config);

            var p = new { Id = Id };
            //Beneficial for Unit testing.
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");

            return output;
        }
    }
}
