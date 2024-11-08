using Contrib.Extensions.Configuration;
using ManagementServer.Settings;

namespace ManagementServer.Configuration;

public static class ApplicationSettings
{
    public static void AddSettings(this IServiceCollection services)
    {
        services.AddOptions<StaticDataSourceSettings>().AutoBind();
    }
}