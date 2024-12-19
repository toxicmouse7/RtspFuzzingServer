using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.Rtcp;

public record SenderReport(
    bool Padding,
    byte ReceptionCount,
    ushort Length,
    long NtpTimestamp,
    uint RTS,
    uint SPC,
    uint SOC)
    : RtcpPacket(Padding, 200, 0x12121212);