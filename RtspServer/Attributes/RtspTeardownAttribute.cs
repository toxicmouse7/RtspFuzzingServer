namespace RtspServer.Attributes;

public class RtspTeardownAttribute : BaseRtspMethodAttribute
{
    public override string SupportedMethod => "TEARDOWN";
}