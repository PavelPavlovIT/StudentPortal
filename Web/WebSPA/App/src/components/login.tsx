import React from 'react';
import { Form, Input, Button, Checkbox, message } from 'antd';
import history from '../service/history';
import { login, saveAuth } from '../service/auth'
import { messageService, authorizationService } from '../service/studentService';

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};

export default class Login extends React.Component {
    state = {
        message: "",
        login: false
    };
    subscriptionMessage: any;
    subscriptionAuth: any;
    componentDidMount() {
        this.subscriptionMessage = messageService.getMessage().subscribe(message => {
            if (message) {
                this.setState({ message: message });
            }
        });

        this.subscriptionAuth = authorizationService.getStatus().subscribe(item => {
            if (item) {
                this.setState({ login: item });
            }
        });
    }

    componentWillUnmount() {
        this.subscriptionMessage.unsubscribe();
        this.subscriptionAuth.unsubscribe();
    }

    onFinish(values: any) {
        login(values.login, values.password)
            .then((data) => {
                if (data.status == 200) {
                    saveAuth(data.userid, data.studentid, data.token, data.login)
                    history.push("/")
                    authorizationService.login();
                } else {
                    message.error('Status:' + data.status + " " + data.message);
                }

            })
    };

    onFinishFailed(errorInfo: any) {

    };
    onRegistration() {
        history.push('/registration');
    }
    render() {
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
                    <Input.Password />
                </Form.Item>

                <Form.Item {...tailLayout}>
                    <Button type="primary" htmlType="submit">
                        Login
              </Button>
                    <Button type="link" htmlType="button" onClick={this.onRegistration}>Registration</Button>
                </Form.Item>
            </Form>
        );
    }
};