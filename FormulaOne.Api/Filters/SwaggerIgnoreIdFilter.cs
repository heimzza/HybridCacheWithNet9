using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FormulaOne.Api.Filters;

public class SwaggerIgnoreIdFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Check if the schema is used for a request
        if (context.Type.Name is "Driver" or "Achievement" && schema.Properties.TryGetValue("id", out var value))
        {
            value.ReadOnly = true; // Mark as read-only
        }
    }
}
