import React, { Component } from 'react';
import { Table, Popconfirm } from 'antd';
import { Link } from 'react-router-dom';
import StudentService from '../service/studentService';
import { Group } from './models/group';

interface IGroupsProps {
  groups: Group[],
}

interface IGroupsState {
  pagination: any
  totalCount: any
  loading: boolean
  groups: Group[];
}

export default class ListGroup extends Component<IGroupsProps, IGroupsState> {
  subscription: any;

  constructor(props: IGroupsProps) {
    super(props)
    this.state = {
      pagination: { pageSize: 3, current: 1 },
      loading: false,
      totalCount: 0,
      groups: []
    };

    this.subscription = StudentService.Groups().subscribe((data: any) => {
      this.setState({
        groups: data.records,
        pagination: {
          total: data.totalRecords === undefined ? 0 : data.totalRecords,
          pageSize: this.state.pagination.pageSize,
          current: 1
        }
      })
    })
  }

  componentDidMount() {
    this.handleTableChange(1, null, null)
  }

  componentWillUnmount() {
    this.subscription.unsubscribe();
  }

  handleDelete(groupId: number): void {
    StudentService.RemoveGroup(groupId);
    this.handleTableChange(1, null, null)
  }

  handleTableChange = (pagination: any, filters: any, sorter: any) => {
    this.setState({ loading: true });
    const pager = { ...this.state.pagination };
    pager.current = pagination.current;
    this.setState({
      pagination: pager,
    });
    StudentService.loadGroups(pagination.current)
      .then((data: any) => {
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
  render() {
    const { groups } = this.state;
    const columns = [
      {
        title: 'Action',
        dataIndex: 'id',
        key: 'action',
        render: (text: any, record: any) => (
          typeof this.state.groups !== 'undefined' ? (
            <Popconfirm title="Sure to delete?" onConfirm={() => this.handleDelete(record.id)}>
              <a>Delete</a>
            </Popconfirm>
          ) : null
        ),
      },
      {
        title: 'Name',
        dataIndex: 'Name',
        key: 'Name',
        render: (text: any, record: any) => (
          <span>
            <Link to={{
              pathname: "/group/update/id=" + record.id,
              key: record.id,
              state: record
            }}> {record.name} </Link>
          </span>
        ),
      }
    ];

    return (
      <React.Fragment>
        <Table
          dataSource={groups}
          columns={columns}
          pagination={this.state.pagination}
          loading={this.state.loading}
          onChange={this.handleTableChange}
          rowKey={record => record.id}
        //  rowKey={record => record.id.toString()}
        />
      </React.Fragment>
    );
  }
}

