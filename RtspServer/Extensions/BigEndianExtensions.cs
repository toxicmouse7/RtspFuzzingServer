namespace RtspServer.Extensions;

public static class BigEndianExtensions
{
    public static byte[] ToBigEndian(this ushort value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        return bytes;
    }
    public static byte[] ToBigEndian(this short value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        return bytes;
    }
    
    public static byte[] ToBigEndian(this uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        return bytes;
    }
    
    public static byte[] ToBigEndian(this int value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        return bytes;
    }
    
    public static byte[] ToBigEndian(this long value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);

        return bytes;
    }
}