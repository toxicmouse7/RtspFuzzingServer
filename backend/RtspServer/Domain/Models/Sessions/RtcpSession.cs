using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Abstract;

namespace RtspServer.Domain.Models.Sessions;

public class RtcpSession
{
    private readonly ISender _sender;
    private readonly IRtcpPacketSource _packetSource;
    private Task? _playTask;
    private CancellationTokenSource _cts;
    
    public RtcpSession(RtspSession rtspSession, IRtcpPacketSource rtcpPacketSource, ISender sender)
    {
        RtspSession = rtspSession;
        _packetSource = rtcpPacketSource;
        _sender = sender;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(RtspSession.Token);
    }
    
    private RtspSession RtspSession { get; }
    private CancellationToken Token => _cts.Token;

    public void Play()
    {
        _playTask = Task.Run(async () =>
        {
            while (!Token.IsCancellationRequested)
            {
                var packet = await _packetSource.GetPacketAsync();
                var sendPacketCommand = new SendRtcpPacketCommand(packet, RtspSession.RTPEndPoint);
                await _sender.Send(sendPacketCommand, Token);
            }
        }, Token);
    }

    public void Pause()
    {
        Task.Run(async () =>
        {
            if (_playTask is not null)
            {
                await _cts.CancelAsync();
                await _playTask;
                _cts.Dispose();
                _cts = CancellationTokenSource.CreateLinkedTokenSource(RtspSession.Token);
            }
        }, CancellationToken.None);
    }
}