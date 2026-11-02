using System.Globalization;
using DirectoryService.Web.Configurations;
using Serilog;
using Shared.Framework.Middleware;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddDependencies(builder.Configuration);
    
    var app = builder.Build();
    app.UseAppMiddlewares();
    app.Configure();
    app.MapControllers();
    await app.RunSeedAsync();
    await app.RunAsync();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally {
   await Log.CloseAndFlushAsync();
}

namespace DirectoryService.Web
{
    public partial class Program;
}