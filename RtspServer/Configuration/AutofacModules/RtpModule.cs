using Autofac;
using RtspServer.Abstract;
using RtspServer.Services;

namespace RtspServer.Configuration.AutofacModules;

public class RtpModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RTPStreamingService>()
            .As<IRTPStreamingService>()
            .SingleInstance();
    }
}