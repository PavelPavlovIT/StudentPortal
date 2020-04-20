import React, { Component, useState } from 'react';
import { Student } from './models/student';
import { Form, Input, Button, Select, Tag, Checkbox } from 'antd';
import { FormInstance } from 'antd/lib/form/Form';
import history from '../service/history';

import StudentService from '../service/studentService';
import { SizeType } from 'antd/lib/config-provider/SizeContext';
import { getID, getStudentID, setStudent } from '../service/auth';
import { Group } from './models/group';

const { Option } = Select;

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};

interface ItemStudentProps {
    location: any;
}

interface ItemStudentState {
    id: number;
    isMale: boolean;
    firstName: string;
    lastName: string;
    patronymic: string;
    nick: string;
    error: string;
    componentsize: SizeType;
    groupsAll: Group[];
    groups: Group[];
}

export default class ItemStudent extends Component<ItemStudentProps, ItemStudentState> {
    formRef?: any = React.createRef<FormInstance>();
    subscription: any;
    constructor(props: any) {
        super(props)
        this.state = {
            id: 0,
            isMale: true,
            firstName: "",
            lastName: "",
            patronymic: "",
            nick: "",
            error: "200",
            componentsize: 'small',
            groupsAll: [],
            groups: new Array<Group>()
        };
    }

    componentDidMount() {
        const student = new Student(this.props.location.state);
        let _groups: Group[];
        let groupIds: number[];
        groupIds = student.groups.map((item: Group) => {
            return item.id
        })
        StudentService.loadAllGroups()
            .then((data) => {
                _groups = data.map((item: any) => {
                    return new Group(item)
                })
                this.setState({ groupsAll: _groups })
            })
        this.formRef.current.setFieldsValue({
            id: student.id,
            gender: student.isMale ? 'male' : 'female',
            firstName: student.firstName,
            lastName: student.lastName,
            patronymic: student.patronymic,
            nick: student.nick,
            groups: groupIds
        });
        this.setState({
            id: student.id,
            isMale: student.isMale,
            firstName: student.firstName,
            lastName: student.lastName,
            patronymic: student.patronymic,
            nick: student.nick,
            error: "200",
            componentsize: 'small',
            groups: student.groups
        })
    }

    componentDidUpdate(prevProps: any, prevState: ItemStudentState) {
    }

    componentWillUnmount() {

    }

    onReset = () => {
        if (this.formRef.current != null)
            this.formRef.current.resetFields();
    };

    onFinish = async (values: any) => {
        if (this.state.id == 0) {
            const student = new Student(values);
            await StudentService.AddStudent(student, values.groups)
                .then((res: any) => {
                    if (res.status == "200") {
                        history.push('/');
                        return res.context
                    } else {
                        this.setState({
                            error: res.context
                        })
                    }
                })
        } else {
            const student = new Student(values);
            await StudentService.UpdateStudent(student, values.groups)
                .then((res: any) => {
                    if (res.status == "200") {
                        setStudent(getID(), student.id)
                            .then((res) => {
                            })
                        history.push('/');
                    } else {
                        this.setState({
                            id: student.id,
                            isMale: student.isMale,
                            firstName: student.firstName,
                            lastName: student.lastName,
                            patronymic: student.patronymic,
                            nick: student.nick,
                            error: res.context
                        })
                    }
                });
        }
    };

    handleChangeGroup(value: any) {
    }

    render() {
        const student = new Student(this.props.location.state);
        const itsme = student.id == getStudentID() ? <Checkbox checked>Its me</Checkbox> : (student.id == 0 ? '' : <Checkbox >Its me</Checkbox>)
        if (student.id == 0) {
            this.onReset();
        }
        const err = (this.state.error == "200") ? false : true
        let tagError;
        if (err) {
            tagError = <Tag color="red">{this.state.error}</Tag>;
        }
        let Groups: JSX.Element[];
        Groups = this.state.groupsAll.map((item: Group) => {
            return <Option value={item.id}>{item.name}</Option>
        })
        return (
            <div>
                <Form
                    {...layout}
                    name="control-ref"
                    ref={this.formRef}
                    onFinish={this.onFinish}
                    wrapperCol={{ sm: 7 }}
                >
                    {tagError}
                    <Form.Item name="id" >
                        <Input hidden={true} />
                    </Form.Item>
                    <Form.Item name="gender" label="Gender" rules={[{ required: true, message: 'Please select gender' }]}>
                        <Select >
                            <Option value="male">Male</Option>
                            <Option value="female">Female</Option>
                        </Select>
                    </Form.Item>
                    <Form.Item name="firstName" label="First Name"
                        rules={[
                            { required: true, message: 'Please input your first name!' },
                            { max: 40, message: 'First name must be maximum 40 characters.' }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item name='lastName' label="Last Name"
                        rules={[
                            { required: true, message: 'Please input your last name!' },
                            { max: 40, message: 'Last name must be maximum 40 characters.' }]} >
                        <Input />
                    </Form.Item>
                    <Form.Item label="Patronymic" name='patronymic' rules={[{ max: 60, message: 'Patronymic must be maximum 60 characters.' }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item name='nick' label="Nick"
                        rules={[
                            { min: 6, message: 'Nick must be minimum 6 characters.' },
                            { max: 16, message: 'Nick must be maximum 16 characters.' }]}>
                        <Input />
                    </Form.Item>
                    <Form.Item name="groups" label="Groups">
                        <Select
                            mode="multiple"
                            style={{ width: '100%' }}
                            placeholder="Please select"
                            onChange={this.handleChangeGroup}
                        >
                            {Groups}
                        </Select>
                    </Form.Item>
                    <Form.Item {...tailLayout} name="me" >
                        {itsme}
                    </Form.Item>

                    <Form.Item {...tailLayout}>
                        <Button type="primary" htmlType="submit">Save</Button>
                    </Form.Item>
                </Form>
            </div>
        );
    }
}