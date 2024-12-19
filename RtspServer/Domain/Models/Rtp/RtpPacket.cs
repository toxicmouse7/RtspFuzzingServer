using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.Rtp;

public record RtpPacket(RtpHeader Header, RtpContentHeader ContentHeader, byte[] Content);