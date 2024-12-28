using System.Net;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManagementServer.Configuration;
using ManagementServer.Configuration.AutofacModules;
using ManagementServer.Configuration.MappingConfigurations;
using ManagementServer.Domain.Abstract;
using ManagementServer.Hubs;
using ManagementServer.Infrastructure;
using ManagementServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RtspServer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

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
        containerBuilder.RegisterModule<DefaultModule>();
    });

builder.Services.AddSettings();

builder.Services.AddDbContext<ApplicationContext>();
builder.Services
    .AddRtspServer(new IPEndPoint(IPAddress.Any, 554), opt =>
    {
        opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    })
    .AddRtpPacketSourceFactory<RtpPacketSourceManager>()
    .AddRtcpPacketSourceFactory<RtcpPacketSourceManager>();

builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile<RestProfile>();
    opt.AddProfile<DatabaseProfile>();
    opt.AddProfile<ApplicationProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.Services.GetRequiredService<ApplicationContext>().Database.Migrate();
}

app.MapControllers();

app.MapHub<SessionsHub>("/sessions_hub");
app.MapHub<FuzzingHub>("/fuzzing_hub");

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();