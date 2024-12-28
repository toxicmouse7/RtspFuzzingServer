using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspResponses;

public record RtspOptionsResponse(IReadOnlyCollection<string> Methods, long Sequence) : RtspResponse(Sequence);