using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Models.RtspResponses;

public record RtspDescribeResponse(
    SessionDescriptionProtocol.SessionDescriptionProtocol SessionDescriptionProtocol,
    long Sequence)
    : RtspResponse(Sequence);