namespace TestUtility;

public record RtpPacket(RtpHeader Header, RtpJpegHeader ContentHeader, byte[] Content);