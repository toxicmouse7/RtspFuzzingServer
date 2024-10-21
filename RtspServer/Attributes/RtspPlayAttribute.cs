namespace RtspServer.Attributes;

public class RtspPlayAttribute : BaseRtspMethodAttribute
{
    public override string SupportedMethod => "PLAY";
}