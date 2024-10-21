namespace RtspServer.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RtspOptionsAttribute : BaseRtspMethodAttribute
{
    public override string SupportedMethod => "OPTIONS"; 
}