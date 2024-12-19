using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Abstract;

namespace RtspServer.Domain.Models.Sessions;

public class RtpSession
{
    private readonly ISender _sender;
    private readonly IRtpPacketSource _packetSource;
    private Task? _playTask;
    private CancellationTokenSource _cts;
    
    public RtpSession(RtspSession rtspSession, ISender sender, IRtpPacketSource packetSource)
    {
        RtspSession = rtspSession;
        _packetSource = packetSource;
        _sender = sender;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(rtspSession.Token);
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
                var sendPacketCommand = new SendRtpPacketCommand(packet, RtspSession.RTPEndPoint);
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