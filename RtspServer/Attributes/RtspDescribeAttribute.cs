namespace RtspServer.Attributes;

public class RtspDescribeAttribute : BaseRtspMethodAttribute
{
    public override string SupportedMethod => "DESCRIBE";
}