using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManagementServer.Configuration;
using ManagementServer.Configuration.AutofacModules;
using RtspServer.Configuration.AutofacModules;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(
    containerBuilder =>
    {
        containerBuilder.RegisterModule<RtspModule>();
        containerBuilder.RegisterModule<RtpModule>();
        containerBuilder.RegisterModule<DefaultModule>();
    });

builder.Services.AddSettings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();