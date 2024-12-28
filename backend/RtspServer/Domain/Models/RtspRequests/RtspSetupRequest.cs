using System.Net;
using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspRequests;

public record RtspSetupRequest(IReadOnlyDictionary<string, string> Headers, IPAddress Address)
    : RtspRequest(Headers, Address)
{
    public string[] Transport => Headers["Transport"].Split(';');
}