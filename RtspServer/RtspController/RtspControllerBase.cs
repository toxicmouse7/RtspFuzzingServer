using System.Reflection;
using RtspServer.Attributes;
using RtspServer.RtspResponses;
using SessionDescriptionProtocol = RtspServer.RtspResponses.SessionDescriptionProtocol;

namespace RtspServer.RtspController;

public class RtspControllerBase
{
    [RtspOptions]
    public IRtspResponse Options()
    {
        var supportedMethods = GetType()
            .GetMethods()
            .Select(mi => mi.GetCustomAttribute(typeof(BaseRtspMethodAttribute)) as BaseRtspMethodAttribute)
            .Where(attr => attr is not null)
            .Select(attr => attr!.SupportedMethod)
            .ToArray();

        return new Ok(new Dictionary<string, string>
        {
            ["Public"] = string.Join(", ", supportedMethods)
        });
    }
    
    public static IRtspResponse NotImplemented()
    {
        return new NotImplemented();
    }

    protected IRtspResponse SessionDescriptionProtocol(SDP.SessionDescriptionProtocol protocol)
    {
        return new SessionDescriptionProtocol(protocol);
    }

    protected IRtspResponse Ok(Dictionary<string, string>? headers = null)
    {
        return new Ok(headers);
    }
}