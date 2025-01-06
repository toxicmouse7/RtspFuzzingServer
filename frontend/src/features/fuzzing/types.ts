import type {RtpHeader, RtpJpegHeader} from "../sessions/types.ts";

export type FuzzingPreset = {
    id: string,
    generatedPayloads: number,
    rtpHeader: RtpHeader,
    rtpJpegHeader: RtpJpegHeader
};

export type OutFuzzingPreset = {
    rtpHeader: RtpHeader,
    rtpJpegHeader: RtpJpegHeader
};