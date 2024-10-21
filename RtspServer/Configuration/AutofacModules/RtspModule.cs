using Autofac;
using Microsoft.Extensions.Hosting;
using RtspServer.Rtsp;

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
    }
}