export type Session = {
    id: string
};

export type RtpHeader = {
    hasExtensionHeader: boolean,
    hasPadding: boolean,
    extensionHeaderLength: number,
    marker: boolean,
    payloadType: number,
    csrcCount: number,
    timestamp: number | null,
    sequenceNumber: number | null
}

export type RtpJpegHeader = {
    typeSpecific: number,
    fragmentOffset: number,
    type: number,
    quantization: number,
    width: number,
    height: number
}