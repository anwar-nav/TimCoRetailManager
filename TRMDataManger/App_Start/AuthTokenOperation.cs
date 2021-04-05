#if DEBUG

using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TRMDataManger.App_Start
{
    public class AuthTokenOperation : IDocumentFilter
    {
        /// <summary>
        /// This method will add a new route of the API to its documentation with a post command.
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiExplorer"></param>
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {
                    //Category to be used will be Auth
                    tags = new List<string> { "Auth" },
                    //Data being pass to post command should be of this type e.g. in message body
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    //Definition of Parameters
                    parameters = new List<Parameter>
                    {
                        new Parameter
                        {
                            type="string",
                            name = "grant_type",
                            required = true,
                            @in = "formData",// this is the textbox created in the form and value provided will be used a parameter value
                            @default = "password"
                        },
                        new Parameter
                        {
                            type="string",
                            name = "username",
                            required = false,
                            @in = "formData"// this is the textbox created in the form and value provided will be used a parameter value
                        },
                        new Parameter
                        {
                            type="string",
                            name = "password",
                            required = false,
                            @in = "formData"// this is the textbox created in the form and value provided will be used a parameter value
                        },
                    }
                }
            });
        }
    }
}

#endif
