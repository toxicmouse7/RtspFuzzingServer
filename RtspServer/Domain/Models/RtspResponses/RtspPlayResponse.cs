using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspResponses;

public record RtspPlayResponse(long SessionId, long Sequence) : RtspResponse(Sequence);