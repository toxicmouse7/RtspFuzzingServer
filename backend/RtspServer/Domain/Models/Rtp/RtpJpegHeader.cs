using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.Rtp;

public record RtpJpegHeader(
    byte TypeSpecific,
    byte[] FragmentOffset,
    byte Type,
    byte Quantization,
    byte Width,
    byte Height) : RtpContentHeader;