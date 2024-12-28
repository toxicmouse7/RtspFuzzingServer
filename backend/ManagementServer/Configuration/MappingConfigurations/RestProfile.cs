using AutoMapper;
using ManagementServer.Domain.Models;
using ManagementServer.Settings;

namespace ManagementServer.Configuration.MappingConfigurations;

public class RestProfile : Profile
{
    public RestProfile()
    {
        CreateMap<RtspServer.Domain.Models.Sessions.RtspSession, Dto.Rest.Out.Session>();

        CreateMap<Dto.Rest.RTPHeader, RtspServer.Domain.Models.Rtp.RtpHeader>()
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.Extension),
                opt => opt.MapFrom(s => s.HasExtensionHeader))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.Marker), opt => opt.MapFrom(s => s.Marker))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.Padding), opt => opt.MapFrom(s => s.HasPadding))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.SequenceNumber),
                opt => opt.MapFrom(s => s.SequenceNumber))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.Timestamp),
                opt => opt.MapFrom(s => s.Timestamp))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.PayloadType),
                opt => opt.MapFrom(s => s.PayloadType))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.HeaderExtensionLength),
                opt => opt.MapFrom(s => s.ExtensionHeaderLength))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.CSRCCount),
                opt => opt.MapFrom(s => s.CSRCCount))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.CSRC),
                opt => opt.MapFrom(_ => Array.Empty<byte>()))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpHeader.SSRCIdentifier),
                opt => opt.MapFrom(_ => 0x12121212))
            .ForAllMembers(opt => opt.Ignore());

        CreateMap<Dto.Rest.RTPJpegHeader, RtspServer.Domain.Models.Rtp.RtpJpegHeader>()
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpJpegHeader.Height), opt => opt.MapFrom(s => s.Height))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpJpegHeader.Width), opt => opt.MapFrom(s => s.Width))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpJpegHeader.Quantization),
                opt => opt.MapFrom(s => s.Quantization))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpJpegHeader.Type), opt => opt.MapFrom(s => s.Type))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpJpegHeader.TypeSpecific),
                opt => opt.MapFrom(s => s.TypeSpecific))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpJpegHeader.FragmentOffset),
                opt => opt.MapFrom(s => MapFragmentOffset(s.FragmentOffset)))
            .ForAllMembers(opt => opt.Ignore());

        CreateMap<RtspServer.Domain.Models.Rtp.RtpJpegHeader, Dto.Rest.RTPJpegHeader>()
            .ForMember(d => d.Quantization, opt => opt.MapFrom(s => s.Quantization))
            .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type))
            .ForMember(d => d.TypeSpecific, opt => opt.MapFrom(s => s.TypeSpecific))
            .ForMember(d => d.Width, opt => opt.MapFrom(s => s.Width))
            .ForMember(d => d.Height, opt => opt.MapFrom(s => s.Height))
            .ForMember(d => d.FragmentOffset, opt => opt.MapFrom(s => MapFragmentOffset(s.FragmentOffset)));

        CreateMap<Dto.Rest.RTPJpegHeader, RtspServer.Domain.Models.Abstract.RtpContentHeader>()
            .ConvertUsing((s, _, ctx) => ctx.Mapper.Map<RtspServer.Domain.Models.Rtp.RtpJpegHeader>(s));

        CreateMap<RtspServer.Domain.Models.Abstract.RtpContentHeader, Dto.Rest.RTPJpegHeader>()
            .ConvertUsing((s, _, ctx) =>
                ctx.Mapper.Map<Dto.Rest.RTPJpegHeader>((RtspServer.Domain.Models.Rtp.RtpJpegHeader)s));

        CreateMap<Dto.Rest.RTPPacket, RtspServer.Domain.Models.Rtp.RtpPacket>()
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpPacket.Header), opt => opt.MapFrom(s => s.RTPHeader))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpPacket.ContentHeader),
                opt => opt.MapFrom(s => s.RTPJpegHeader))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpPacket.Content),
                opt => opt.MapFrom(s => MapPayload(s.Base64Content)))
            .ForAllMembers(opt => opt.Ignore());

        CreateMap<RtspServer.Domain.Models.Rtp.RtpHeader, Dto.Rest.RTPHeader>()
            .ForMember(d => d.HasExtensionHeader, opt => opt.MapFrom(s => s.Extension))
            .ForMember(d => d.HasPadding, opt => opt.MapFrom(s => s.Padding))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp))
            .ForMember(d => d.SequenceNumber, opt => opt.MapFrom(s => s.SequenceNumber))
            .ForMember(d => d.PayloadType, opt => opt.MapFrom(s => s.PayloadType))
            .ForMember(d => d.Marker, opt => opt.MapFrom(s => s.Marker))
            .ForMember(d => d.ExtensionHeaderLength, opt => opt.MapFrom(s => s.HeaderExtensionLength))
            .ForMember(d => d.CSRCCount, opt => opt.MapFrom(s => s.CSRCCount));

        CreateMap<RtpFuzzingPreset, Dto.Rest.Out.RTPPacket>()
            .ForMember(d => d.RTPHeader, opt => opt.MapFrom(s => s.Header))
            .ForMember(d => d.RTPJpegHeader, opt => opt.MapFrom(s => s.ContentHeader))
            .ForMember(d => d.Base64Content, opt => opt.MapFrom(s => MapPayload(s.Payload)));

        CreateMap<Dto.Rest.RTPPacket, RtpFuzzingPreset>()
            .ForCtorParam(nameof(RtpFuzzingPreset.Header),
                opt => opt.MapFrom((s, ctx) => ctx.Mapper.Map<RtspServer.Domain.Models.Rtp.RtpHeader>(s.RTPHeader)))
            .ForCtorParam(nameof(RtpFuzzingPreset.ContentHeader),
                opt => opt.MapFrom((s, ctx) =>
                    ctx.Mapper.Map<RtspServer.Domain.Models.Abstract.RtpContentHeader>(s.RTPJpegHeader)))
            .ForCtorParam(nameof(RtpFuzzingPreset.Payload),
                opt => opt.MapFrom(s => MapPayload(s.Base64Content)))
            .ForCtorParam(nameof(RtpFuzzingPreset.AppendSettings),
                opt => opt.MapFrom(s => MapAppendSettings(s)))
            .ForAllMembers(opt => opt.Ignore());
        
        CreateMap<Dto.Rest.SenderReport, RtspServer.Domain.Models.Rtcp.SenderReport>()
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.Padding), 
                opt => opt.MapFrom(s => s.Padding))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.ReceptionCount),
                opt => opt.MapFrom(s => s.ReceptionReportCount))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.Length),
                opt => opt.MapFrom(s => s.Length))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.NtpTimestamp),
                opt => opt.MapFrom(s => s.NtpTimestamp))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.RTS),
                opt => opt.MapFrom(s => s.RtpTimestamp))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.SPC),
                opt => opt.MapFrom(s => s.SenderPacketsCount))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtcp.SenderReport.SOC),
                opt => opt.MapFrom(s => s.SenderOctetsCount))
            .ForAllMembers(opt => opt.Ignore());

        CreateMap<RtpFuzzingPreset, Dto.Rest.Out.RtpFuzzingPreset>()
            .IncludeBase<RtpFuzzingPreset, Dto.Rest.Out.RTPPacket>()
            .ForMember(d => d.GeneratedPayloads, opt => opt.MapFrom(s => s.RawFuzzingData.Count));
            
        
        CreateMap<Dto.Rest.SenderReport, RtspServer.Domain.Models.Abstract.RtcpPacket>()
            .ConvertUsing((s, _, ctx) => ctx.Mapper.Map<RtspServer.Domain.Models.Rtcp.SenderReport>(s));
    }

    private static byte[]? MapPayload(string? payload)
    {
        return payload is null ? null : Convert.FromBase64String(payload);
    }

    private static string? MapPayload(byte[]? payload)
    {
        return payload is null ? null : Convert.ToBase64String(payload);
    }

    private static int MapFragmentOffset(byte[] fragmentOffset)
    {
        return BitConverter.ToInt32(fragmentOffset.Append((byte)0).ToArray());
    }

    private static byte[] MapFragmentOffset(int fragmentOffset)
    {
        return BitConverter.GetBytes(fragmentOffset)[..3];
    }

    private static AppendSettings MapAppendSettings(Dto.Rest.RTPPacket rtpPacket)
    {
        return new AppendSettings
        {
            UseOriginalPayload = rtpPacket.Base64Content is null,
            UseOriginalTimestamp = rtpPacket.RTPHeader.Timestamp is null,
            UseOriginalSequence = rtpPacket.RTPHeader.SequenceNumber is null
        };
    }
}