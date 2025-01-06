/* eslint-disable @typescript-eslint/no-explicit-any */
import {HttpTransportType, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {useEffect} from "react";

export type Action = {
    method: string,
    callback: (...args: any[]) => any;
}

const useSignalR = (hubUrl: string, actions: Action[]) => {
    const connection = new HubConnectionBuilder()
        .withUrl(hubUrl, {
            transport: HttpTransportType.WebSockets,
            skipNegotiation: true
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

    useEffect(() => {
        actions.forEach(a => connection.on(a.method, a.callback));
        connection.start();

        return () => {
            actions.forEach(a => connection.off(a.method));
            connection.stop();
        };
    }, []);
};

export default useSignalR;