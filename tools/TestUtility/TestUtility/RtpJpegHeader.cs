namespace TestUtility;

public record RtpJpegHeader(
    byte TypeSpecific,
    byte[] FragmentOffset,
    byte Type,
    byte Quantization,
    byte Width,
    byte Height);