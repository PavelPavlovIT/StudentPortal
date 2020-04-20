import React from 'react';
import { Link } from 'react-router-dom';
import { Table, Modal, Popconfirm, Tag, PageHeader, Input, Form } from 'antd';
import { Student } from './models/student';
import StudentService from '../service/studentService';
import history from '../service/history';
import { isLogged, getStudentID } from '../service/auth';
import FilterStudent from './filterStudent';

interface IStudentsProps {
    getListStudents?: any;
    removeStudent?: any;
    students?: any;
    location: any;
}

interface IStudentsState {
    visible?: boolean;
    selectedId?: number;
    redirect?: boolean;
    students: Student[]
    pagination: any
    totalCount: any
    loading: boolean
    my: boolean
    message: ""
    test?: any
}

export default class ListStudents extends React.Component<IStudentsProps, IStudentsState> {
    subscription: any;
    filter: any;
    sorter: any;
    constructor(props: Readonly<IStudentsProps>, context?: IStudentsState) {
        super(props)
        this.state = {
            students: [],
            pagination: { pageSize: 3, current: 1 },
            loading: false,
            totalCount: 0,
            my: false,
            message: ""
        };
        this.filter = {
            search: '',
            gender: 'all'
        }

        this.sorter = {}
        this.subscription = StudentService.Students().subscribe((data: any) => {
            this.setState({
                students: data.records === undefined ? [] : data.records,
                pagination: {
                    total: data.totalRecords === undefined ? 0 : data.totalRecords,
                    pageSize: this.state.pagination.pageSize,
                    current: 1
                }
            })
        })
    }

    componentDidUpdate(prevProps: any, prevState: any) {
    }

    componentDidMount() {
        this.handleTableChange(1, null, null)
    }

    componentWillUnmount() {
        this.subscription.unsubscribe();
    }

    removeStudent(id: number) {

    }

    showModal = (studentId: number) => {
        this.setState({
            visible: true,
            selectedId: studentId
        });
    };

    handleOk = (e: any) => {
        this.props.removeStudent(this.state.selectedId)
        this.setState({
            visible: false,
        });
        this.props.getListStudents();
    };

    handleCancel = (e: any) => {
        this.setState({
            visible: false,
        });
    };

    handleDelete(studentId: number): void {
        const students = [...this.state.students];
        StudentService.RemoveStudent(studentId);
    }

    handleTableChange = (pagination: any, filters: any, sorter: any) => {
        this.setState({ loading: true });
        const pager = { ...this.state.pagination };
        pager.current = pagination.current;
        this.setState({
            pagination: pager,
        });
        let isMale: boolean | undefined;
        let orderBy: string | undefined;
        let desc: boolean | undefined;
        let my: boolean = false;
        isMale = this.filter.gender == 'all' ? undefined : (this.filter.gender == 'male' ? true : false);
        if (sorter !== null) {
            orderBy = sorter.field;
            desc = sorter.order == 'ascend' ? true : false;
            this.sorter = sorter;
        } else this.sorter = {}
        if (this.props.location.state == 'my') {
            my = true;
        } else {
            my = false;
        }
        StudentService.loadStudent(pagination.current, my, this.filter.search, isMale, orderBy, desc)
            .then((data: any) => {
                if (data.status == 401)
                    return history.push('/login');

                const pagination = { ...this.state.pagination };
                const total = data.totalRecords == undefined ? 0 : data.totalRecords
                pagination.pageSize = data.pageSize;
                pagination.total = total;
                pagination.current = data.currentPage + 1;
                this.setState({
                    totalCount: total,
                    loading: false,
                    pagination: pagination,

                });
            })
    };
    onFilter(filter: any) {
        this.filter = {
            search: filter.search,
            gender: filter.gender
        }
        this.handleTableChange(1, null, null)
    }

    render() {
        const { students } = this.state;
        const columns = [
            {
                title: '',
                dataIndex: 'id',
                key: 'id',
                visible: false,
                render: (text: any, record: any) => (
                    record.id == getStudentID() ? <Tag color="green">its me</Tag> : null
                )
            },
            {
                title: 'Action',
                dataIndex: 'id',
                key: 'action',
                render: (text: any, record: any) => (
                    this.state.students.length >= 1 ? (
                        <Popconfirm title="Sure to delete?" onConfirm={() => this.handleDelete(record.id)}>
                            <a>Delete</a>
                        </Popconfirm>
                    ) : null
                ),
            },
            {
                title: 'Sex',
                dataIndex: 'isMale',
                key: 'isMale',
                render: (isMale: boolean) => (
                    <span>
                        {isMale ? 'Male' : 'Female'}
                    </span>
                ),
                sorter: true
            },
            {
                title: 'First name',
                dataIndex: 'FirstName',
                key: 'firstName',
                render: (text: any, record: Student) => (
                    <span>
                        <Link to={{
                            pathname: "/student/modify",
                            state: record
                        }}>{record.firstName}</Link>
                    </span>
                ),
                sorter: true
            },
            {
                title: 'Last Name',
                dataIndex: 'LastName',
                key: 'lastName',
                render: (text: any, record: Student) => (
                    <span>
                        <Link to={{
                            pathname: "/student/modify",
                            state: record
                        }}>{record.lastName}</Link>
                    </span>
                ),
                sorter: true
            },
            {
                title: 'Patronymic',
                dataIndex: 'Patronymic',
                key: 'patronymic',
                render: (text: any, record: Student) => (
                    <span>
                        <Link to={{
                            pathname: "/student/modify",
                            state: record
                        }}>{record.patronymic}</Link>
                    </span>
                ),
                sorter: true
            },
            {
                title: 'Groups',
                dataIndex: 'Groups',
                key: 'groups',
                render: (text: any, record: Student) => (
                    <span>
                        {
                            record.groups.map((item: any) => {
                                return <Tag color="blue">{item.name}</Tag>
                            })
                        }
                    </span>
                ),
            },
            {
                title: 'Nick',
                dataIndex: 'Nick',
                key: 'nick',
                sorter: true,
                render: (text: any, record: any) => (
                    <span>{record.nick}</span>
                )

            },
        ];

        return (
            <React.Fragment>

                <PageHeader
                    title=''
                    extra={[
                        <div>{"Total count of students: " + this.state.totalCount}</div>
                    ]}
                ></PageHeader>
                <FilterStudent onFilter={(filter: any) => this.onFilter(filter)} filter={this.filter} />
                <Table
                    dataSource={students}
                    columns={columns}
                    pagination={this.state.pagination}
                    loading={this.state.loading}
                    onChange={this.handleTableChange}
                    rowKey={record => record.id}

                />
            </React.Fragment>
        );
    }

}

