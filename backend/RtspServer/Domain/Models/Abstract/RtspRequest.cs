using System.Net;
using RtspServer.Extensions;

namespace RtspServer.Domain.Models.Abstract;

public abstract record RtspRequest(IReadOnlyDictionary<string, string> Headers, IPAddress Address)
{
    public long Sequence => Headers["CSeq"].ToInt64();
}