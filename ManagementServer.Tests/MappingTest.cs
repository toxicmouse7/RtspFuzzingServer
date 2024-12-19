using AutoMapper;
using ManagementServer.Configuration.MappingConfigurations;

namespace ManagementServer.Tests;

public class MappingTest
{
    [Fact]
    public void TestRestMapping()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<RestProfile>());
        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void TestDatabaseMapping()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<DatabaseProfile>());
        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void TestApplicationMapping()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        configuration.AssertConfigurationIsValid();
    }
}