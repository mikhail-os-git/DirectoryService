using System.Globalization;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Configurations;
using DirectoryService.Web.Configurations;
using DirectoryService.Web.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddDependencies(builder.Configuration);
    
    var app = builder.Build();
    app.UseExceptionMiddleware();
    app.Configure();
    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally {
    Log.CloseAndFlush();
}
