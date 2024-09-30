using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RtspServer.Configuration.AutofacModules;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = Host.CreateApplicationBuilder(args);

var appSettingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "AppSettings");
builder.Configuration.SetBasePath(appSettingsPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);

builder.Services.AddLogging(config =>
{
    config.ClearProviders();

    var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration);

    config.AddSerilog(loggerConfiguration.CreateLogger());
});

builder.ConfigureContainer(
    new AutofacServiceProviderFactory(),
    containerBuilder =>
    {
        containerBuilder.RegisterModule<RtspModule>();
    });

var app = builder.Build();

app.Run();