using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container ...

// builder.Services.AddMvc();
// builder.Services.AddControllersWithViews();
builder.Services.AddControllers(options => // just add [controller] from [MVC]
{
    // options.OutputFormatters.Add();
    // options.InputFormatters.Add();
    options.ReturnHttpNotAcceptable = true;
}
).AddXmlDataContractSerializerFormatters(); // add [XML] support in api Headers/Key: Accept, Value: application/xml

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// database connection
builder.Services.AddDataProtection();

// file extention add as a singletone to service provider
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

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
app.Run();
