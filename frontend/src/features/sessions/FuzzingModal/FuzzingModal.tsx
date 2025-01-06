import {Modal, Progress, ProgressProps} from "antd";
import {useAppDispatch, useAppSelector} from "../../../app/hooks.ts";
import {hideFuzzingModal, selectCurrentSessionId, selectFuzzingModalVisible} from "../sessionsSlice.ts";
import {useEffect, useState} from "react";
import {getFuzzingHubUrl, getFuzzingUrl, getStopFuzzingUrl} from "../backapi.ts";
import useSignalR from "../../../app/hooks/useSignalR.ts";
import {Loader} from "../../../components";
import styles from "./FuzzingModal.module.scss";
import axios from 'axios';

const twoColors: ProgressProps['strokeColor'] = {
    '0%': '#108ee9',
    '100%': '#87d068',
};

const FuzzingModal = () => {
    const
        dispatch = useAppDispatch(),
        visible = useAppSelector(selectFuzzingModalVisible),
        sessionId = useAppSelector(selectCurrentSessionId),
        [totalToSend, setTotalToSend] = useState<number>(),
        [error, setError] = useState(false),
        [packetsSent, setPacketsSent] = useState<number>(0);

    const
        stopFuzzing = async () => {
            await axios.post(getStopFuzzingUrl(sessionId!));
        };

    useEffect(() => {
        if (!sessionId || !visible) {
            return;
        }

        const startFuzzing = async () => {
            await axios.post(getFuzzingUrl(sessionId));
        };

        startFuzzing();
    }, [sessionId, visible]);

    useEffect(() => {
        if (!visible) {
            return () => {
                setTotalToSend(undefined);
                setPacketsSent(0);
                setError(false);
            };
        }

        return () => {
        };
    }, [visible]);

    useSignalR(getFuzzingHubUrl(), [
        {
            method: "PreFuzz",
            callback: (total: number) => {
                setTotalToSend(total);
            }
        },
        {
            method: "PacketSent",
            callback: (sent: number) => {
                setPacketsSent(sent);
            }
        },
        {
            method: "Error",
            callback: () => setError(true)
        }
    ])

    const loader = (
        <div className={styles.loader}>
            <Loader/>
        </div>
    );

    const body = (
        <div className={styles.progress}>
            <Progress
                type="dashboard"
                strokeColor={twoColors}
                status={error ? "exception" : undefined}
                percent={parseFloat(((packetsSent! / totalToSend!) * 100).toFixed(2))}
                size={200}
            />
        </div>
    );

    if (!sessionId) {
        return <></>;
    }

    return (
        <Modal
            open={visible}
            footer={<></>}
            onCancel={() => {
                stopFuzzing();
                dispatch(hideFuzzingModal());
            }}
        >
            <div className={styles.modalContainer}>
                {totalToSend === undefined ? loader : body}
                {
                    error &&
                    <span>Failed on {packetsSent}</span>
                }
            </div>
        </Modal>
    );
};

export default FuzzingModal;