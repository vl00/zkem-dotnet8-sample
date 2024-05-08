using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using TestNS.Controllers;
using TestNS.Common;

//-------------------------------------------------------------------------------------------------------
var appBuilder = WebApplication.CreateBuilder(args);
ConfigureAppsettings(appBuilder.Environment, appBuilder.Configuration);
ConfigureLogging(appBuilder.Host, appBuilder.Logging, appBuilder.Configuration);
await ConfigureServices(appBuilder.Services, appBuilder.Configuration);
//-------------------------------------------------------------------------------------------------------
await using var app = appBuilder.Build();
ConfigureAppUse(app);
await app.RunAsync();
//--------------------------------------------------------------------------------------------------------

void ConfigureAppsettings(IWebHostEnvironment environment, IConfigurationBuilder configuration)
{
    
}

void ConfigureLogging(IHostBuilder hostBuilder, ILoggingBuilder builder, IConfiguration configuration)
{
    //builder.ClearProviders();
    
}

static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddOptions();
    services.AddHttpContextAccessor();
    await default(ValueTask);

    services.AddControllersWithViews()
        .AddNewtonsoftJson(opts =>
        {
            opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            opts.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        });

    services.AddSwaggerEx(typeof(TestController), "Server2", "v1");
}

void ConfigureAppUse(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        // ...
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // ...
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();
    app.UseCors();

    

    //app.UseAuthentication();
    

    app.UseRouting();

    // ...

    //app.UseAuthorization();

    app.UseSwaggerEx("Server2", "v1");

    app.MapControllerRoute(
            name: "Area",
            pattern: "{area:exists}/{controller=Values}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapGet("/", () => "Hello World");

    
}

async Task OnApplicationStarting(IServiceProvider services)
{
    
}

async Task OnApplicationStopping(IServiceProvider services)
{
    
}
