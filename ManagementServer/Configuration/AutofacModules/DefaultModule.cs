using Autofac;
using ManagementServer.Domain;
using ManagementServer.Domain.Abstract;
using ManagementServer.Infrastructure;
using RtspServer.Domain.Abstract;

namespace ManagementServer.Configuration.AutofacModules;

public class DefaultModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<RtpPacketSourceManager>(x => (RtpPacketSourceManager)x.Resolve<IRtpPacketSourceFactory>());
        builder.Register<RtcpPacketSourceManager>(x => (RtcpPacketSourceManager)x.Resolve<IRtcpPacketSourceFactory>());
        
        builder.RegisterType<FuzzingService>()
            .As<IFuzzingService>()
            .SingleInstance();

        builder.RegisterType<RtpFuzzingPayloadGeneratorClient>()
            .As<IRtpFuzzingPayloadGenerator>()
            .SingleInstance();
    }
}