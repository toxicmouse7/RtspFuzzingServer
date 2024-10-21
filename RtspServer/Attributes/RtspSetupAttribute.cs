namespace RtspServer.Attributes;

public class RtspSetupAttribute : BaseRtspMethodAttribute
{
    public override string SupportedMethod => "SETUP";
}