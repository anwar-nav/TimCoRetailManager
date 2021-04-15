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
    public class UserData : IUserData
    {
        private readonly ISQLDataAccess _sql;

        public UserData(ISQLDataAccess sql)
        {
            _sql = sql;
        }

        //For usage purposes an anonymous object of type dynamic is passed as the second argument of
        //LoadData method. This usage of dynamic only works if it is in same assembly.
        public List<UserModel> GetUserById(string Id)
        {
            var p = new { Id = Id };
            //Beneficial for Unit testing.
            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");

            return output;
        }
    }
}
