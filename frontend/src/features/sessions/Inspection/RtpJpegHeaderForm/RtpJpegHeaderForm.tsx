import {Form, FormInstance, InputNumber} from "antd";
import {RtpJpegHeader} from "../../types.ts";
import styles from "./RtpJpegHeaderForm.module.scss";

const initialValues: RtpJpegHeader = {
    width: 90,
    height: 60,
    fragmentOffset: 0,
    type: 1,
    quantization: 99,
    typeSpecific: 0
}

type Props = {
    form: FormInstance<RtpJpegHeader>
}

const RtpJpegHeaderForm = ({form}: Props) => {
    return (
        <Form
            form={form}
            initialValues={initialValues}
        >
            <span className={styles.title}>Rtp Jpeg Header</span>

            <Form.Item<RtpJpegHeader>
                name="width"
                label="Width"
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpJpegHeader>
                name="height"
                label="Height"
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpJpegHeader>
                name="fragmentOffset"
                label="Fragment Offset"
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpJpegHeader>
                name="type"
                label="Type"
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpJpegHeader>
                name="quantization"
                label="Quantization"
            >
                <InputNumber/>
            </Form.Item>

            <Form.Item<RtpJpegHeader>
                name="typeSpecific"
                label="Type Specific"
            >
                <InputNumber/>
            </Form.Item>
        </Form>
    );
};

export default RtpJpegHeaderForm;