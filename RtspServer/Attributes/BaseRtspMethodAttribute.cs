namespace RtspServer.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public abstract class BaseRtspMethodAttribute : Attribute
{
    public abstract string SupportedMethod { get; }
}