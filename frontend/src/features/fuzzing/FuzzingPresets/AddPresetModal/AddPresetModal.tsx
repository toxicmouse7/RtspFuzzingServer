import {Button, Form, Modal} from "antd";
import {useAppDispatch, useAppSelector} from "../../../../app/hooks.ts";
import {addPreset, hideAddModal, selectAdding, selectAddModalVisible} from "../../fuzzingSlice.ts";
import RtpHeaderForm from "../../../sessions/Inspection/RtpHeaderForm/RtpHeaderForm.tsx";
import RtpJpegHeaderForm from "../../../sessions/Inspection/RtpJpegHeaderForm/RtpJpegHeaderForm.tsx";
import type {RtpHeader, RtpJpegHeader} from "../../../sessions/types.ts";
import styles from "./AddPresetModal.module.scss";
import {OutFuzzingPreset} from "../../types.ts";
import {Loader} from "../../../../components";

const AddPresetModal = () => {
    const
        dispatch = useAppDispatch(),
        visible = useAppSelector(selectAddModalVisible),
        adding = useAppSelector(selectAdding),
        [rtpHeaderForm] = Form.useForm<RtpHeader>(),
        [rtpJpegHeaderForm] = Form.useForm<RtpJpegHeader>();

    const onAdd = () => {
        const data: OutFuzzingPreset = {
            rtpHeader: rtpHeaderForm.getFieldsValue(),
            rtpJpegHeader: rtpJpegHeaderForm.getFieldsValue()
        };

        dispatch(addPreset(data));
    };

    const body = (
        <div className={styles.forms}>
            <RtpHeaderForm form={rtpHeaderForm}/>
            <RtpJpegHeaderForm form={rtpJpegHeaderForm}/>
        </div>
    );

    const footer = (
        <>
            <Button onClick={onAdd} type="primary">Add</Button>
        </>
    );

    const loader = (
        <div className={styles.loader}>
            <Loader/>
        </div>
    );

    return (
        <Modal
            open={visible}
            onCancel={() => dispatch(hideAddModal())}
            footer={adding ? <></> : footer}
            width="800px"
        >
            {adding ? loader : body}
        </Modal>
    );
};

export default AddPresetModal;