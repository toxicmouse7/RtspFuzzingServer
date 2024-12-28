using System.Net;
using System.Net.Sockets;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Abstract.SessionServices;
using RtspServer.Domain.SessionServices;
using RtspServer.Infrastructure;

namespace RtspServer;

public static class DependencyInjection
{
    public static IServiceCollection AddRtspServer(
        this IServiceCollection serviceCollection,
        IPEndPoint endPoint,
        Action<MediatRServiceConfiguration>? configurator = null)
    {
        serviceCollection.AddSingleton<UdpClient>(_ => new UdpClient());

        serviceCollection.AddMediatR(conf =>
        {
            configurator?.Invoke(conf);
            conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            conf.Lifetime = ServiceLifetime.Singleton;
        });

        serviceCollection.AddSingleton<INetworkService, NetworkService>();
        serviceCollection.AddSingleton<IRtpSessionService, RtpSessionService>();
        serviceCollection.AddSingleton<IRtcpSessionService, RtcpSessionService>();

        serviceCollection.AddSingleton<IRtspSessionsRepo, RtspSessionsRepo>();

        serviceCollection.AddHostedService<Infrastructure.RtspServer>(
            x => ActivatorUtilities.CreateInstance<Infrastructure.RtspServer>(x, endPoint));

        return serviceCollection;
    }

    public static IServiceCollection AddRtpPacketSourceFactory<TPacketSourceFactory>(
        this IServiceCollection serviceCollection)
        where TPacketSourceFactory : class, IRtpPacketSourceFactory
    {
        serviceCollection.AddSingleton<IRtpPacketSourceFactory, TPacketSourceFactory>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddRtcpPacketSourceFactory<TPacketSourceFactory>(
        this IServiceCollection serviceCollection)
        where TPacketSourceFactory : class, IRtcpPacketSourceFactory
    {
        serviceCollection.AddSingleton<IRtcpPacketSourceFactory, TPacketSourceFactory>();

        return serviceCollection;
    }
}