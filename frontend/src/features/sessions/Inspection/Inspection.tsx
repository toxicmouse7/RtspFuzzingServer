import {useParams} from "react-router";
import RtpHeaderForm from "./RtpHeaderForm/RtpHeaderForm.tsx";
import {Button, Form} from "antd";
import {RtpHeader, RtpJpegHeader} from "../types.ts";
import RtpJpegHeaderForm from "./RtpJpegHeaderForm/RtpJpegHeaderForm.tsx";
import styles from "./Inspection.module.scss";
import {getSendRtpUrl} from "../backapi.ts";
import axios from 'axios';

const Inspection = () => {
    const
        {sessionId} = useParams(),
        [rtpHeaderForm] = Form.useForm<RtpHeader>(),
        [rtpJpegHeaderForm] = Form.useForm<RtpJpegHeader>();

    const onSubmit = () => {
        const sendPacket = async () => {
            const data = {
                rtpHeader: rtpHeaderForm.getFieldsValue(),
                rtpJpegHeader: rtpJpegHeaderForm.getFieldsValue(),
            };

            await axios.post(getSendRtpUrl(sessionId!), data);
        };

        sendPacket();
    }

    return (
        <div className={styles.formsContainer}>
            <RtpHeaderForm form={rtpHeaderForm}/>
            <RtpJpegHeaderForm form={rtpJpegHeaderForm}/>
            <Button onClick={onSubmit} type="dashed">
                Submit
            </Button>
        </div>
    );
};

export default Inspection;