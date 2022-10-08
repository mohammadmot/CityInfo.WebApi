using CityInfo.Api.Services;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container ...

// builder.Services.AddMvc();
// builder.Services.AddControllersWithViews();
builder.Services.AddControllers //(); // just add [controller] from [MVC]
(options =>
{
    // - options.OutputFormatters.Add();
    // - options.InputFormatters.Add();
    // * options.ReturnHttpNotAcceptable = true;
}
)
// * .AddXmlDataContractSerializerFormatters() // add [XML] support in api Headers/Key: Accept, Value: application/xml
.AddNewtonsoftJson(); // add json serializer and deserializer [replace json.net]



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// database connection
builder.Services.AddDataProtection();

// file extention add as a singletone to service provider
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#region logging

#region Serilog => Serilog.AspNetCore + Serilog.Sinks.File + Serilog.Sinks.Console
// change default logger to serilog logger:
Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

// add .net core logger
// Inversion of Control IoC
// Dependency Injection = DI
// ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0

// logger create object
// - builder.Services.AddSingleton<ILogger,>();

// configuration of logger
// *** builder.Services.AddLogger(configuration);

// change log config in appsettings.json, appsettings.Development.json
// config log config in code not appsetting
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Filter function
// A filter function is invoked for all providers and categories that don't have rules assigned to
// them by configuration or code:
// The preceding code displays console logs when the category contains Controller or Microsoft
// and the log level is Information or higher.
builder.Logging.AddFilter((provider, category, logLevel) =>
{
    if (provider.Contains("Console")
        && category.Contains("Controller")
        && logLevel >= LogLevel.Information)
    {
        return true;
    }
    else if (provider.Contains("Console")
        && category.Contains("Microsoft")
        && logLevel >= LogLevel.Information)
    {
        return true;
    }
    else
    {
        return false;
    }
});

// ref: https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers
/*
Here are some third-party logging frameworks that work with various .NET workloads:
    elmah.io(GitHub repo)
    Gelf(GitHub repo)
    JSNLog(GitHub repo)
    KissLog.net(GitHub repo)
    Log4Net(GitHub repo)
    NLog(GitHub repo)
    NReco.Logging(GitHub repo)
    Sentry(GitHub repo)
    Serilog(GitHub repo)
    Stackdriver(GitHub repo)
*/

#endregion

#region mail custom service
builder.Services.AddScoped<LocalMailService>();
#endregion

// finaly build application
var app = builder.Build();

// =========================================
// [Middlewares] ...
bool bTest = false;

if (bTest)
{
    #region Pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    #endregion

    #region my custome middleware
    // sample of simple application life cycle
    // responce to other requests ...
    app.Run(async (contex) =>
    {
        await contex.Response.WriteAsync($"Hello Request: {contex.Request.Path}\r\nPage not found to render request !!!");
    });
    #endregion

    app.Run();
}

#region Pipeline

// Configure the HTTP request [pipeline].
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// http redirect to https
app.UseHttpsRedirection();

#region routing
// routing: Controller/Action/id?
// addressing controllers, default path:
// domain.com/{controller=Home}/{action=Index}/{id?}
// Routing to controller actions in ASP.NET Core:
//      https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-6.0
// Routing in ASP.NET Core:
//      https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-6.0
app.UseRouting();

// add endpoint(s)
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
} );
#endregion

// 
// app.UseStaticFiles();

// 
// app.UseAuthorization();

// user auth middleware:
// app.UseAuthentication();

#endregion

// =========================================

#region log
app.Logger.LogInformation("### Adding Routes");
app.MapGet("/", () => "### Hello App in Root !");
app.Logger.LogInformation("### Starting the app ...");
#endregion

app.Run();
