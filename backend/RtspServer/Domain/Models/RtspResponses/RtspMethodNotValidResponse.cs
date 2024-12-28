using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspResponses;

public record RtspMethodNotValidResponse(long Sequence) : RtspResponse(Sequence);