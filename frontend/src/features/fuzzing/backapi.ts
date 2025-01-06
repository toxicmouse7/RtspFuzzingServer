import {getBackendUrl} from "../sessions/backapi.ts";

export const
    getFuzzingPresetsUrl = () => `${getBackendUrl()}/Management/rtp_presets`,
    getFuzzingPresetUrl = (id: string) => `${getBackendUrl()}/Management/rtp_presets/${id}`;