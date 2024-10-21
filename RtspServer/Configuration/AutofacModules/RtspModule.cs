using Autofac;
using Microsoft.Extensions.Hosting;
using RtspServer.Abstract;
using RtspServer.Rtsp;
using RtspServer.Services;

namespace RtspServer.Configuration.AutofacModules;

public class RtspModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Rtsp.RtspServer>()
            .SingleInstance()
            .As<IHostedService>()
            .AutoActivate();

        builder.RegisterType<RtspRequestParser>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<RtspConnectionContext>()
            .AsSelf();
        
        builder.RegisterType<RtspController.RtspController>()
            .AsSelf();

        builder.RegisterType<SessionService>()
            .As<ISessionService>()
            .SingleInstance();
    }
}