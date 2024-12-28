namespace RtspServer.Extensions;

public static class StringExtensions
{
    public static long ToInt64(this string str) => long.Parse(str);
    public static int ToInt32(this string str) => int.Parse(str);
    public static short ToInt16(this string str) => short.Parse(str);
    public static ushort ToUInt16(this string str) => ushort.Parse(str);
}