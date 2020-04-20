import React from 'react';
import { Form, Input, Button, Checkbox, Tag, message } from 'antd';
import { login, saveAuth, register } from '../service/auth'
import history from '../service/history';
const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};

interface IRegistrationProps {
    // login: any;
}

interface IRegistrationState {
    pass: string;
    confirm: string;
}

export default class Registration extends React.Component<IRegistrationProps, IRegistrationState> {
    constructor(props: any) {
        super(props)
        this.OnChangePassword = this.OnChangePassword.bind(this);
        this.OnChangeConfirm = this.OnChangeConfirm.bind(this);

        this.state = { pass: "", confirm: "" };
    }
    onFinish(values: any) {
        register(values.login, values.password)
            .then((data) => {
                if (data.status==200) {
                    message.success('Registration completed successfully.');
                    history.push("/login")
                } else {
                    message.error('Status:' + data.status + " " + data.message);
                    // message.error('Warning: An error occurred while registering !!!');
                }
            })
    };

    onFinishFailed(errorInfo: any) {

    };
    OnChangePassword(e: any) {
        this.setState({
            pass: e.target.value
        })

    }
    OnChangeConfirm(e: any) {
        this.setState({
            confirm: e.target.value
        })

    }

    render() {
        let tagError;
        let regButton = <Button type="primary" htmlType="submit" >Registration</Button>;
        if (this.state.pass !== this.state.confirm) {
            tagError = <Tag color="red">Your password and confirmation password do not match.</Tag>;
            regButton = <Button type="primary" htmlType="submit" disabled>Registration</Button>;
        }

        return (
            <Form
                {...layout}
                name="basic"
                initialValues={{ remember: true }}
                onFinish={this.onFinish}
                onFinishFailed={this.onFinishFailed}
            >
                <Form.Item
                    label="Login"
                    name="login"
                    rules={[{ required: true, message: 'Please input your username!' }]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    label="Password"
                    name="password"
                    rules={[{ required: true, message: 'Please input your password!' }]}
                >
                    <Input.Password onChange={this.OnChangePassword} />
                    {/* {tagError} */}
                </Form.Item>
                <Form.Item
                    label="Confirm password"
                    name="confirmPassword"
                // rules={[{ required: true, message: 'Please input confirm your password!' }]}
                >
                    <Input.Password onChange={this.OnChangeConfirm} />
                    {tagError}
                </Form.Item>
                <Form.Item {...tailLayout} >
                    {regButton}
                </Form.Item>
            </Form>
        );
    }
};