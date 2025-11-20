using Microsoft.OpenApi.Models;

namespace API.Configuration;

public static class OpenApiConfiguration
{
    public static void ConfigureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                // Add description about authentication
                document.Info.Description = @"
⚠️ **Authentication Required**: Most endpoints require authentication.

**To authenticate:**
1. First call `POST /login` with your group slug to get the authentication cookie
2. Include the `X-SessionId` header in all requests
3. The cookie will be automatically included in requests from the browser
";

                // Add X-SessionId as a required parameter to all operations
                var guid = Guid.NewGuid().ToString();
                foreach (var path in document.Paths)
                {
                    foreach (var operation in path.Value.Operations)
                    {
                        operation.Value.Parameters ??= new List<OpenApiParameter>();
                        operation.Value.Parameters.Add(new OpenApiParameter
                        {
                            Name = "X-SessionId",
                            In = ParameterLocation.Header,
                            Required = true,
                            Description = "Unique session identifier",
                            Schema = new OpenApiSchema
                            {
                                Type = "string",
                                Default = new Microsoft.OpenApi.Any.OpenApiString(guid)
                            }
                        });
                    }
                }

                return Task.CompletedTask;
            });
        });
    }
}

