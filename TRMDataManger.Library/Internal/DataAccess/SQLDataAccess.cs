using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManger.Library.Internal.DataAccess
{
    /// <summary>
    /// This will access database and have generic methods for loading and saving data.
    /// </summary>
    internal class SQLDataAccess
    {
        //This method will take the name of connection as parameter and return connection string.
        public string GetConnectionString(string name)
        {
            //This will return the connection string fetched from config file.
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        //This generic method is synchronous and will load data in type T list fetched from database using Dapper.
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            //Connection string of database.
            string connectionString = GetConnectionString(connectionStringName);

            //Sql connection is made and query is executed and the result is saved in List of T type and returned.
            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        //This generic method is synchronous and will save data in database using Dapper.
        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            //Connection string of database.
            string connectionString = GetConnectionString(connectionStringName);

            //Sql connection is made and query is executed to save data in database using storedprocedure.
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
