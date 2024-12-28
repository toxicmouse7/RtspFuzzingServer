using System.Net;
using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspRequests;

public record RtspPlayRequest(long SessionId, IReadOnlyDictionary<string, string> Headers, IPAddress Address) 
    : RtspRequest(Headers, Address);