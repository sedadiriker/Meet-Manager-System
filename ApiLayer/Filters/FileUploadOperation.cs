using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Tüm parametreleri kontrol et
        var formDataParameters = context.MethodInfo.GetParameters()
            .Where(p => p.GetCustomAttributes(typeof(FromFormAttribute), false).Any() &&
                        p.ParameterType == typeof(IFormFile))
            .Select(p => new OpenApiParameter
            {
                Name = p.Name,
                In = ParameterLocation.Header, // Header yerine FormData kullanmak gerekebilir
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                }
            })
            .ToList();

        foreach (var parameter in formDataParameters)
        {
            operation.Parameters.Add(parameter);
        }
    }
}
