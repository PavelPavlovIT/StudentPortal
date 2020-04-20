import React, { Component } from 'react';

import { Form, Button, Select } from 'antd';
import { Input } from 'antd';
import { FormInstance } from 'antd/lib/form/Form';
import './filterStudent.css';

const { Option } = Select;

interface IFilterStudentProps {
    onFilter: any;
    filter?: any;
}


export default class FilterStudent extends Component<IFilterStudentProps, any> {
    formRef?: any = React.createRef<FormInstance>();
    filter: any = {};
    constructor(props: any) {
        super(props)
        this.onChangeGender = this.onChangeGender.bind(this)
        this.onFinish = this.onFinish.bind(this)
        this.state = {
            search: '',
            gender: 'all'
        }
        this.filter.search = '';
        this.filter.gender = 'all';
    }

    componentDidMount() {
        this.formRef.current.setFieldsValue({
            search: this.filter.search === 'undefined' ? '' : this.filter.search,
            gender: this.filter.gender === 'undefined' ? 'all' : this.filter.gender,

        });
    }

    componentWillUnmount() {

    }

    onChangeGender(value: any) {
        this.filter.gender = value;
        this.props.onFilter(this.filter)
    }

    onFinish = (values: any) => {
        this.filter.search = values.search;
        this.props.onFilter(values)
    }
    render() {
        const { Search } = Input;
        return (
            <Form
                layout='inline'
                ref={this.formRef}
                onFinish={this.onFinish}
            >
                <Form.Item label="Filter first name" name="search">
                    <Search
                        placeholder="first name filter"
                    />
                </Form.Item>
                <Form.Item name="gender" label="Gender" >
                    <Select
                        onChange={this.onChangeGender}
                        style={{ width: 120 }}
                    >
                        <Option value="all">All</Option>
                        <Option value="male">Male</Option>
                        <Option value="female">Female</Option>
                    </Select>
                </Form.Item>
                <Form.Item className='itemHide'>
                    <Button type="primary" htmlType="submit" ></Button>
                </Form.Item>
            </Form>
        );
    }
}