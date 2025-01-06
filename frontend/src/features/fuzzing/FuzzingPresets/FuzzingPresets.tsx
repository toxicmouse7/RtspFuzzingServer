import {loadPresets, selectPresets, showAddModal} from "../fuzzingSlice.ts";
import {useAppDispatch, useAppSelector} from "../../../app/hooks.ts";
import {useEffect} from "react";
import {Button, Card, List, Switch} from "antd";
import {Loader} from "../../../components";
import type {FuzzingPreset} from "../types.ts";
import styles from "./FuzzingPresets.module.scss";
import {PlusSquareTwoTone, DeleteTwoTone} from "@ant-design/icons"
import AddPresetModal from "./AddPresetModal/AddPresetModal.tsx";
import {getFuzzingPresetUrl} from "../backapi.ts";
import axios from 'axios';

const FuzzingPresets = () => {
    const
        dispatch = useAppDispatch(),
        presets = useAppSelector(selectPresets);

    const onDelete = async (id: string) => {
        await axios.delete(getFuzzingPresetUrl(id));

        dispatch(loadPresets());
    };

    useEffect(() => {
        if (!presets) {
            dispatch(loadPresets());
        }
    }, []);

    const getTitle = (item: FuzzingPreset) => (
        <div className={styles.titleContainer}>
            <div className={styles.title}>
                <span>{item.id}</span>
                <span>Generated payloads: {item.generatedPayloads}</span>
            </div>
            <Button onClick={() => onDelete(item.id)} icon={<DeleteTwoTone/>}/>
        </div>
    );

    if (!presets) {
        return <div className={styles.loader}>
            <Loader/>
        </div>
    }

    return (
        <div className={styles.globalContainer}>
            <List
                dataSource={presets}
                grid={{
                    column: 3,
                    gutter: 16
                }}
                split
                renderItem={(item) => (
                    <List.Item>
                        <Card
                            title={getTitle(item)}
                            className={styles.card}
                        >
                            <div className={styles.container}>
                                <span className={styles.pairTitle}>Rtp Header</span>
                                <div className={styles.pair}>
                                    <span>Extension Header</span>
                                    <Switch disabled value={item.rtpHeader.hasExtensionHeader}/>
                                </div>

                                <div className={styles.pair}>
                                    <span>Extension Header Length</span>
                                    <span>{item.rtpHeader.extensionHeaderLength}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Padding</span>
                                    <Switch disabled value={item.rtpHeader.hasPadding}/>
                                </div>

                                <div className={styles.pair}>
                                    <span>Marker</span>
                                    <Switch disabled value={item.rtpHeader.marker}/>
                                </div>

                                <div className={styles.pair}>
                                    <span>Payload Type</span>
                                    <span>{item.rtpHeader.payloadType}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>CSRC Count</span>
                                    <span>{item.rtpHeader.csrcCount}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Timestamp</span>
                                    <span>{item.rtpHeader.timestamp ?? "null"}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Sequence</span>
                                    <span>{item.rtpHeader.sequenceNumber ?? "null"}</span>
                                </div>
                            </div>

                            <div className={styles.container}>
                                <span className={styles.pairTitle}>Rtp Jpeg Header</span>

                                <div className={styles.pair}>
                                    <span>Width</span>
                                    <span>{item.rtpJpegHeader.width}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Height</span>
                                    <span>{item.rtpJpegHeader.height}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Type</span>
                                    <span>{item.rtpJpegHeader.type}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Type Specific</span>
                                    <span>{item.rtpJpegHeader.typeSpecific}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Quantization</span>
                                    <span>{item.rtpJpegHeader.quantization}</span>
                                </div>

                                <div className={styles.pair}>
                                    <span>Fragment Offset</span>
                                    <span>{item.rtpJpegHeader.fragmentOffset}</span>
                                </div>
                            </div>
                        </Card>
                    </List.Item>
                )}
            />

            <div className={styles.buttonContainer}>
                <Button
                    className={styles.button}
                    icon={<PlusSquareTwoTone className={styles.icon}/>}
                    onClick={() => dispatch(showAddModal())}
                />
            </div>

            <AddPresetModal/>
        </div>
    );
};

export default FuzzingPresets;