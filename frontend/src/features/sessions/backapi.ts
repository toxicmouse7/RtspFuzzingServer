export const
    getBackendUrl = () => import.meta.env.VITE_BACKEND_URL,
    getSessionsHubUrl = () => `${getBackendUrl()}/sessions_hub`,
    getFuzzingHubUrl = () => `${getBackendUrl()}/fuzzing_hub`,
    getSessionsUrl = () => `${getBackendUrl()}/Management/sessions`,
    getSendRtpUrl = (id: string) => `${getBackendUrl()}/Management/send_rtp?sessionId=${id}`,
    getSessionUrl = (id: string) => `${getSessionsUrl()}?sessionId=${id}`,
    getFuzzingUrl = (id: string) => `${getBackendUrl()}/Management/start_rtp_fuzzing?sessionId=${id}`,
    getStopFuzzingUrl = (id: string) => `${getBackendUrl()}/Management/stop_rtp_fuzzing?sessionId=${id}`;