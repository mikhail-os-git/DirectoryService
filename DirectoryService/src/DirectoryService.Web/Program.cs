using System.Globalization;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Configurations;
using DirectoryService.Web.Configurations;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddDependencies(builder.Configuration);
    
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DirectoryService"));  
        
    }
    
    app?.MapControllers();

    app?.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally {
    Log.CloseAndFlush();
}
