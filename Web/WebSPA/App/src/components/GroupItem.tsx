import React, { Component } from 'react';
import { Group } from './models/group';
import { Form, Input, Button} from 'antd';
import { FormInstance } from 'antd/lib/form/Form';
import StudentService from '../service/studentService';
import history from '../service/history';

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};


interface IItemGroupProps {
    location: any;
}

interface IItemGroupState {
    isNew: boolean;
    group: Group;
}

export default class ItemGroup extends Component<IItemGroupProps, IItemGroupState> {
    formRef?: any = React.createRef<FormInstance>();
    constructor(props: any) {
        super(props)
        this.state = {
            isNew: false,
            group: new Group("")
        };
    }

    componentDidMount() {
        const _group = new Group(this.props.location.state);
        let arr = this.props.location.pathname.split('/');
        this.setState({
            group: _group,
            isNew: arr[2] == "update" ? false : true
        })
        this.formRef.current.setFieldsValue({
            id: _group.id,
            name: _group.name,
        });
    }

    componentWillUnmount() {
    }

    onFinish(values: any) {
        if (values.id == 0) {
            StudentService.AddGroup(new Group(values))
                .then((res: any) => {
                    history.push('/group/all');
                })
        } else {
            StudentService.UpdateGroup(new Group(values))
                .then((res: any) => {
                    history.push('/group/all');
                })
        }
    }

    render() {
        return (
            <div>
                <Form
                    {...layout}
                    name="control-ref"
                    ref={this.formRef}
                    onFinish={this.onFinish}
                    wrapperCol={{ sm: 7 }}
                >
                    <Form.Item name="id" >
                        <Input hidden={true} />
                    </Form.Item>
                    <Form.Item name="name" label="Name"
                        rules={[
                            { required: true, message: 'Please input name group' },
                            { max: 25, message: 'First name must be maximum 25 characters.' }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item {...tailLayout}>
                        <Button type="primary" htmlType="submit">Save</Button>
                    </Form.Item>
                </Form>
            </div>
        );
    }
}

