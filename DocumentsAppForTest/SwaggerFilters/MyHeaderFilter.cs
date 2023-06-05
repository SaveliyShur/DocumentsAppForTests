using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ShopAppForTest.Middlewares
{
    public class MyHeaderFilter : IOperationFilter
    {
        private readonly string _name;

        public MyHeaderFilter(string headerName)
        {
            _name = headerName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (operation.Tags.Any(t => t.Name == "User"))
            {
                if (operation.Parameters.Any(p => p.Name == "userId" || p.Name == "id"))
                {
                    operation.Parameters.FirstOrDefault(p => p.Name == "userId" || p.Name == "id").Example = new OpenApiString("1d712bf9-7c30-4d16-bdc4-b27d4f6aa27d");

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = _name,
                        In = ParameterLocation.Header,
                        Required = true,
                        Example = new OpenApiString("0ae54932fdbc4a4a9c52e7e25a7e8f07"),
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    });

                    return;
                }

                if (operation.RequestBody != null && operation.RequestBody.Content.TryGetValue("application/json", out var value))
                {
                    if (value.Schema.Reference.Id == "DeleteUserModel")
                    {
                        operation.Parameters.Add(new OpenApiParameter
                        {
                            Name = _name,
                            In = ParameterLocation.Header,
                            Required = true,
                            Example = new OpenApiString("0ae54932fdbc4b4a4c52e7e25a7e8f21"),
                            Schema = new OpenApiSchema
                            {
                                Type = "string"
                            }
                        });

                        return;
                    }

                    if (value.Schema.Reference.Id == "CreateUserModel" && operation.Parameters.Count == 0)
                    {
                        operation.Parameters.Add(new OpenApiParameter
                        {
                            Name = _name,
                            In = ParameterLocation.Header,
                            Required = true,
                            Example = new OpenApiString("TestDefaultToken"),
                            Schema = new OpenApiSchema
                            {
                                Type = "string"
                            }
                        });

                        return;
                    }
                }
            }

            if (operation.Tags.Any(t => t.Name == "Documents"))
            {
                if (operation.Parameters.Count == 0)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = _name,
                        In = ParameterLocation.Header,
                        Required = true,
                        Example = new OpenApiString("0ae54932fdbc4a4a9c52e7e25a7e8f07"),
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    });

                    return;
                }
                if (operation.Parameters.Count == 1)
                {
                    if (operation.Responses.TryGetValue("200", out var value))
                    {
                        var content = value.Content?.FirstOrDefault().Value?.Schema?.Type == "boolean";
                        if (content)
                        {
                            operation.Parameters.FirstOrDefault(p => p.Name == "id").Example = new OpenApiString("d0a924df-0039-4cac-8169-c71809b3cafe");
                            operation.Parameters.Add(new OpenApiParameter
                            {
                                Name = _name,
                                In = ParameterLocation.Header,
                                Required = true,
                                Example = new OpenApiString("0ae54932fdbc4a4a9c52e7e25a7e8f07"),
                                Schema = new OpenApiSchema
                                {
                                    Type = "string"
                                }
                            });

                            return;
                        }
                        else
                        {
                            operation.Parameters.FirstOrDefault(p => p.Name == "id").Example = new OpenApiString("492a8111-c781-4d49-a680-d031220e227d");
                            operation.Parameters.Add(new OpenApiParameter
                            {
                                Name = _name,
                                In = ParameterLocation.Header,
                                Required = true,
                                Example = new OpenApiString("0ae54932fdbc4a4a9c52e7e25a7e8f07"),
                                Schema = new OpenApiSchema
                                {
                                    Type = "string"
                                }
                            });

                            return;
                        }
                    }
                }

                if (operation.Parameters.Count == 2)
                {
                    operation.Parameters.FirstOrDefault(t => t.Name == "userId").Example = new OpenApiString("eb0a70fc-e3ff-4c17-b995-a0af61786e89");
                    operation.Parameters.FirstOrDefault(t => t.Name == "documentId").Example = new OpenApiString("492a8111-c781-4d49-a680-d031220e227d");

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = _name,
                        In = ParameterLocation.Header,
                        Required = true,
                        Example = new OpenApiString("0ae54932fdbc4a4a9c52e7e25a7e8f07"),
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    });

                    return;
                }
            }

            if (operation.Tags.Any(t => t.Name == "Auth"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = _name,
                    In = ParameterLocation.Header,
                    Required = true,
                    Example = new OpenApiString("TestDefaultToken"),
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });

                return;
            }

        }
    }
}
