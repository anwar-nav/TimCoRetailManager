using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TRMApi.Models
{
    /// <summary>
    /// This will add a parameter to every operation e.g get, post, put, delete
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        //public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        //{
        //    if (operation.parameters == null) //since the operator get has no parameter it will be null and not empty so comparison is made by ==
        //    {
        //        operation.parameters = new List<Parameter>(); //this instantiates the parameter and hence it will not remain null
        //    }

        //    operation.parameters.Add(new Parameter // new is used for instantiation
        //    {
        //        name = "Authorization",
        //        @in = "header", //where we will put this parameter
        //        description = "access token", //this will show up in documentation
        //        required = false,
        //        type = "string"
        //    });
        //}

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) //since the operator get has no parameter it will be null and not empty so comparison is made by ==
            {
                operation.Parameters = new List<OpenApiParameter>(); //this instantiates the parameter and hence it will not remain null
            }

            operation.Parameters.Add(new OpenApiParameter // new is used for instantiation
            {
                Name = "Authorization",
                In = ParameterLocation.Header,//where we will put this parameter
                Description = "Access Token", //this will show up in documentation
                Required = false,
                Schema = new OpenApiSchema 
                { 
                    Type = "String",
                    //Default = "Bearer "
                }
            });
        }
    }
}
