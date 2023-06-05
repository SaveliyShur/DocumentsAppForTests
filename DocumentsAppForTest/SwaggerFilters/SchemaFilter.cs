using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ShopAppForTest.Models.Api;
using ShopAppForTest.Models.Api.User;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ShopAppForTest.SwaggerFilters
{
    public class SchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(AuthUserModel))
            {
                schema.Example = new OpenApiObject()
                {
                    ["UserName"] = new OpenApiString("AuthUser"),
                    ["Password"] = new OpenApiString("qwerty123"),
                };
            }

            if (context.Type == typeof(CreateUserModel))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Name"] = new OpenApiString(Guid.NewGuid().ToString("N")),
                    ["LastName"] = new OpenApiString(Guid.NewGuid().ToString("N")),
                    ["Patronymic"] = new OpenApiString(Guid.NewGuid().ToString("N")),
                    ["UserName"] = new OpenApiString(Guid.NewGuid().ToString("N")),
                    ["Password"] = new OpenApiString(Guid.NewGuid().ToString("N")),
                    ["Age"] = new OpenApiInteger(20),
                    ["UserPersonalInfo"] = new OpenApiObject(),
                };
            }

            if (context.Type == typeof(DeleteUserModel))
            {
                schema.Example = new OpenApiObject()
                {
                    ["UserId"] = new OpenApiString("eb0a70fc-e3ff-4c17-b995-a0af61716a89"),
                    ["UserSecretId"] = new OpenApiString("1"),
                };
            }

            if (context.Type == typeof(CreateDocumentModel))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Title"] = new OpenApiString("Title"),
                    ["DocumentInBase64"] = new OpenApiString("absdfddfdfdfdfdfdfdfdfdfsdf123asdf"),
                    ["Description"] = new OpenApiString("Description"),
                    ["Sha256HashCode"] = new OpenApiString("Sha256HashCode"),
                };
                
            }
        }
    }
}
