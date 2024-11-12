using System.Runtime.InteropServices;
using FFmpeg.AutoGen.Abstractions;
using RtspServer.Abstract;
using RtspServer.Infrastructure.RTP;

namespace ManagementServer.Infrastructure.WebcamDataSource;

public class MacosWebcamDataSource : IDataSource
{
    private readonly unsafe AVFormatContext* _formatContext;
    private readonly unsafe AVCodecContext* _decoderContext;
    private readonly unsafe AVCodecContext* _encoderContext;
    private readonly int _streamIndex;

    public unsafe MacosWebcamDataSource()
    {
        ffmpeg.avdevice_register_all();
        var format = ffmpeg.av_find_input_format("avfoundation");
        var formatContext = ffmpeg.avformat_alloc_context();

        AVDictionary* options = null;
        ffmpeg.av_dict_set_int(&options, "framerate", 30, 0);
        ffmpeg.av_dict_set(&options, "video_size", "1920x1080", 0);
        ffmpeg.av_dict_set(&options, "pixel_format", "bgr0", 0);

        ffmpeg.avformat_open_input(&formatContext, "0", format, &options);


        ffmpeg.avformat_find_stream_info(formatContext, &options);

        AVCodec* codec = null;
        var streamIndex = ffmpeg.av_find_best_stream(formatContext, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0);

        var stream = formatContext->streams[streamIndex];
        var decoder = ffmpeg.avcodec_alloc_context3(codec);

        ffmpeg.avcodec_parameters_to_context(decoder, stream->codecpar);

        ffmpeg.avcodec_open2(decoder, codec, null);

        _formatContext = formatContext;
        _decoderContext = decoder;

        var encoderCodec = ffmpeg.avcodec_find_encoder(AVCodecID.AV_CODEC_ID_MJPEG);
        var encoderContext = ffmpeg.avcodec_alloc_context3(encoderCodec);

        encoderContext->width = 1920;
        encoderContext->height = 1080;
        encoderContext->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUVJ420P;
        encoderContext->time_base = new AVRational { num = 1, den = 90000 };

        ffmpeg.avcodec_open2(encoderContext, encoderCodec, null);

        _encoderContext = encoderContext;
        _streamIndex = streamIndex;
    }

    public unsafe Task<byte[]> GetStreamableDataAsync()
    {
        var packet = ffmpeg.av_packet_alloc();
        var frame = ffmpeg.av_frame_alloc();

        if (ffmpeg.av_read_frame(_formatContext, packet) < 0)
        {
            return Task.FromResult(Array.Empty<byte>());
        }

        if (packet->stream_index != _streamIndex)
        {
            return Task.FromResult(Array.Empty<byte>());
        }

        ffmpeg.avcodec_send_packet(_decoderContext, packet);
        ffmpeg.avcodec_receive_frame(_decoderContext, frame);
        
        ffmpeg.avcodec_send_frame(_encoderContext, frame);
        ffmpeg.avcodec_receive_packet(_encoderContext, packet);
        
        var binaryData = new byte[packet->size];
        fixed (byte* ptr = binaryData)
        {
            Buffer.MemoryCopy(packet->data, ptr, packet->size, packet->size);
        }
        
        ffmpeg.av_packet_free(&packet);
        ffmpeg.av_frame_free(&frame);
        
        var rtpJpegHeader = new RTPJpegHeader
        {
            Type = 1,
            FragmentOffset = [0, 0, 0],
            Height = 135,
            Width = 240,
            Q = 99,
            TypeSpecific = 0,
        };

        return Task.FromResult(rtpJpegHeader.ToByteArray().Concat(binaryData).ToArray());
    }
}