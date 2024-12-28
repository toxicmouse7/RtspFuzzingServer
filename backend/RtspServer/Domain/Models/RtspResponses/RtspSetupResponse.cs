using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspResponses;

public record RtspSetupResponse(long SessionId, string Transport, long Sequence) : RtspResponse(Sequence);