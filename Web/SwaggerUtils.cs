using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace TestNS.Common;

internal static class SwaggerUtils
{
    public static void AddSwaggerEx(this IServiceCollection services, Type anyTypeInAssembly, string name, string version)
    {
#if DEBUG
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo
            {
                Title = name,
                Version = version,
                Description = "hahaha~~"
            });
            var basePath = Path.GetDirectoryName(anyTypeInAssembly.Assembly.ManifestModule.FullyQualifiedName);
            var files = Directory.EnumerateFiles(basePath, "*.xml");
            foreach (var file in files)
            {
                c.IncludeXmlComments(file, true);
            }
            c.DocInclusionPredicate((docName, description) => true);
        });
        services.AddSwaggerGenNewtonsoftSupport();
#endif
    }

    public static void UseSwaggerEx(this IApplicationBuilder app, string name, string version)
    {
#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // '/swagger'
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{name} {version}");
        });
#endif
    }
}