using Flurl;
using Microsoft.AspNetCore.SignalR.Client;

var apiAddress = Environment.GetEnvironmentVariable("API_ADDRESS");

var fuzzingHub = new HubConnectionBuilder()
    .WithUrl(apiAddress.AppendPathSegment("fuzzing_hub"))
    .Build();

var totalPacketsToSend = 0;
var packetsSent = 0;
    
fuzzingHub.On<int>("PreFuzz", val =>
{
    totalPacketsToSend = val;
});

fuzzingHub.On<int>("PacketSent", val =>
{
    packetsSent = val;
});

fuzzingHub.On("Error", () =>
{
    Console.Error.WriteLine("Failed");
    Environment.Exit(-1);
});

await fuzzingHub.StartAsync();