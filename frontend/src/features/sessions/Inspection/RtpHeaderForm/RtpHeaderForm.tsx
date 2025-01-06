import {Form, FormInstance, InputNumber, Switch} from "antd";
import type {RtpHeader} from "../../types.ts";
import styles from "./RtpHeaderForm.module.scss";

type Props = {
    form: FormInstance<RtpHeader>
    className?: string
}

const initialValues: RtpHeader = {
    hasExtensionHeader: false,
    hasPadding: false,
    extensionHeaderLength: 0,
    marker: true,
    csrcCount: 0,
    payloadType: 26,
    sequenceNumber: null,
    timestamp: null
};

const RtpHeaderForm = ({form, className}: Props) => {
    return (
        <Form
            form={form}
            initialValues={initialValues}
            className={className}
        >
            <span className={styles.title}>Rtp Header</span>

            <Form.Item<RtpHeader>
                label={"Extension"}
                name="hasExtensionHeader"
            >
                <Switch/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"Extension header length"}
                name="extensionHeaderLength"
                rules={[{type: "number", max: 65535, min: 0}]}
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"Padding"}
                name="hasPadding"
            >
                <Switch/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"Marker"}
                name="marker"
            >
                <Switch/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"CSRC Count"}
                name="csrcCount"
                rules={[{type: "number", max: 15, min: 0}]}
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"Payload Type"}
                name="payloadType"
                rules={[{type: "number", max: 127, min: 0}]}
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"Timestamp"}
                name="timestamp"
                rules={[{type: "number", max: Math.pow(2, 32) - 1, min: 0}]}
            >
                <InputNumber placeholder={"NULL"}/>
            </Form.Item>

            <Form.Item<RtpHeader>
                label={"Sequence number"}
                name="sequenceNumber"
                rules={[{type: "number", max: 65535, min: 0}]}
            >
                <InputNumber placeholder={"NULL"}/>
            </Form.Item>
        </Form>
    );
};

export default RtpHeaderForm;