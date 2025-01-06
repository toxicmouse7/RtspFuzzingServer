import {useAppDispatch, useAppSelector} from "../../app/hooks.ts";
import {
    addSession,
    loadSessions,
    removeSession,
    selectSessions,
    setSessions,
    showFuzzingModal
} from "./sessionsSlice.ts";
import {useEffect} from "react";
import {Button, List, Skeleton} from "antd";
import {DeleteTwoTone, EditTwoTone, BugTwoTone} from "@ant-design/icons";
import useSignalR from "../../app/hooks/useSignalR.ts";
import {getSessionsHubUrl} from "./backapi.ts";
import {Session} from "./types.ts";
import {Loader} from "../../components";
import styles from "./Sessions.module.scss"
import {useNavigate} from "react-router";
import FuzzingModal from "./FuzzingModal/FuzzingModal.tsx";

const Sessions = () => {
    const
        navigate = useNavigate(),
        dispatch = useAppDispatch(),
        sessions = useAppSelector(selectSessions)

    const
        onRemove = (id: string) => {
            dispatch(removeSession(id));
        },
        onInspect = (id: string) => {
            navigate(`${id}`);
        },
        onFuzzing = (id: string) => {
            dispatch(showFuzzingModal(id));
        }

    const getActions = (id: string) => [
        <Button onClick={() => onInspect(id)} type="link" icon={<EditTwoTone/>}/>,
        <Button onClick={() => onFuzzing(id)} type="link" icon={<BugTwoTone/>}/>,
        <Button onClick={() => onRemove(id)} type="link" icon={<DeleteTwoTone/>}/>,
    ];

    useEffect(() => {
        if (!sessions) {
            dispatch(loadSessions());
        }
    }, [])

    useSignalR(getSessionsHubUrl(), [
        {
            method: "NewSession",
            callback: (session: Session) => {
                dispatch(addSession(session));
            }
        },
        {
            method: "RemoveSession",
            callback: (sessionId: string) => {
                if (sessions) {
                    dispatch(setSessions(sessions.filter(s => s.id !== sessionId)));
                }
            }
        }
    ]);

    if (!sessions) {
        return <div className={styles.loader}>
            <Loader/>
        </div>;
    }

    return (
        <>
            <List
                dataSource={sessions}
                renderItem={(item) => (
                    <List.Item
                        actions={getActions(item.id)}
                    >
                        <Skeleton active loading={false}>
                            <List.Item.Meta
                                title={item.id}
                                description="RTSP Session"
                            />
                        </Skeleton>
                    </List.Item>
                )}
            />
            <FuzzingModal/>
        </>
    );
};

export default Sessions;