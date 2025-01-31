using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.SignalR.Client;
using TestUtility;

var apiAddress = Environment.GetEnvironmentVariable("API_ADDRESS");

var fuzzingHub = new HubConnectionBuilder()
    .WithUrl(apiAddress.AppendPathSegment("fuzzing_hub"))
    .Build();

var totalPacketsToSend = 0;
var sent = -1;

fuzzingHub.On<int>("PreFuzz", val =>
{
    Console.WriteLine($"{val} packets to send");
    totalPacketsToSend = val;
});

fuzzingHub.On<int>("PacketSent", val =>
{
    Console.WriteLine($"{val}/{totalPacketsToSend} packets sent");
    sent = val;
});

fuzzingHub.On("Error", () =>
{
    Console.Error.WriteLine("Failed");
    Environment.Exit(-1);
});

await fuzzingHub.StartAsync();

var sessions = await apiAddress
    .AppendPathSegments("Management", "sessions")
    .GetJsonAsync<List<Session>>();

var testSession = sessions.First();

await apiAddress
    .AppendPathSegments("Management", "start_rtp_fuzzing")
    .AppendQueryParam("sessionId", testSession.Id)
    .PostAsync();

while (sent != totalPacketsToSend)
{
    await Task.Delay(1000);
}