using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspResponses;

public record RtspTeardownResponse(long Sequence) : RtspResponse(Sequence);