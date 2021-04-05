#if DEBUG

using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TRMDataManger.App_Start
{
    /// <summary>
    /// This will add a parameter to every operation e.g get, post, put, delete
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null) //since the operator get has no parameter it will be null and not empty so comparison is made by ==
            {
                operation.parameters = new List<Parameter>(); //this instantiates the parameter and hence it will not remain null
            }

            operation.parameters.Add(new Parameter // new is used for instantiation
            {
                name = "Authorization",
                @in = "header", //where we will put this parameter
                description = "access token", //this will show up in documentation
                required = false,
                type = "string"
            });
        }
    }
}

#endif