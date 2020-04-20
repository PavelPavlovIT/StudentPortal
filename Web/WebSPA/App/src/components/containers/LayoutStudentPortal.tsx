import { Layout, Menu, Popconfirm, Tag } from 'antd';
import React from 'react';
import { Switch, Link } from 'react-router-dom';
import Routing from '../../routes/route';
import StudentService, { messageService, authorizationService } from '../../service/studentService';
import { isLogged, clearAuth, getLogin } from '../../service/auth'
import history from '../../service/history';
import {
    DesktopOutlined,
    LoginOutlined,
    LogoutOutlined,
    FileOutlined,
    TeamOutlined,
    UserOutlined,
    GroupOutlined,
    UserAddOutlined,
    UsergroupAddOutlined

} from '@ant-design/icons';
import { Group } from '../models/group';

const { Header, Content, Footer, Sider } = Layout;
const { SubMenu } = Menu;

interface IProps {

}

interface IState {
    collapsed: boolean,
    message: string,
    login: boolean,
    groups: Group[]
}

class LayoutStudentPortal extends React.Component<IProps, IState> {
    subscriptionMessage: any;
    subscriptionAuth: any;
    constructor(props: IProps) {
        super(props)
        this.state = {
            collapsed: false,
            message: "",
            login: false,
            groups: []
        };
    }

    componentDidMount() {
        this.subscriptionAuth = authorizationService.getStatus().subscribe(item => {
            this.setState({ login: item });
        });
    }
    componentWillUnmount() {
        this.subscriptionAuth.unsubscribe();
    }


    onCollapse = (collapsed: any) => {
        this.setState({ collapsed });
    };

    login() {

    }

    Logout() {
        clearAuth();
        authorizationService.logout();
        history.push("/login")
    }
    loadGroups() {
        StudentService.loadGroups(1)
            .then((data: any) => {
                //this.setState({ groups: data })
            })
    }

    loadStudents() {
        StudentService.loadStudent(1, false, '', undefined, undefined, undefined)
    }

    loadMyStudents() {
        StudentService.loadStudent(1, true, '', undefined, undefined, undefined)
    }
    render() {
        const isLogout = isLogged() ?
            <Menu.Item key="1">
                <LogoutOutlined />
                <Popconfirm title="Sure to log out?" onConfirm={() => this.Logout()} >
                    <a>Log out ({getLogin()})</a>
                </Popconfirm>
            </Menu.Item>
            :
            <Menu.Item key="login">
                <LoginOutlined />
                <Link to="/login" onClick={this.login}>Log in</Link>
            </Menu.Item>;
        let Students;
        let Groups;
        if (isLogged()) {
            Students =
                <SubMenu
                    key="students"
                    title={
                        <span>
                            <span>Students</span>
                        </span>
                    }>
                    <Menu.Item key="all_students">
                        <UserOutlined />
                        <Link to={{
                            pathname: "/students",
                            state: 'all',
                        }} onClick={this.loadStudents} >All students</Link>
                    </Menu.Item>
                    <Menu.Item key="my_students">
                        <UserOutlined />
                        <Link to={{
                            pathname: "/mystudents",
                            state: 'my',
                            key: 'itsme'
                        }} onClick={this.loadMyStudents} >My students</Link>
                    </Menu.Item>
                    <Menu.Item key="new_student">
                        <UserAddOutlined />
                        <Link to="/student/new">New student</Link>
                    </Menu.Item>
                </SubMenu>
            Groups =
                <SubMenu
                    key="group"
                    title={
                        <span>

                            <span>Groups</span>
                        </span>
                    }>
                    <Menu.Item key="all_group">
                        <GroupOutlined />
                        <Link
                            to={{
                                pathname: "/group/all",
                                state: { groups: this.state.groups }
                            }}
                            onClick={this.loadGroups} > All groups</Link>
                    </Menu.Item>
                    <Menu.Item key="new_group">
                        <UsergroupAddOutlined />
                        <Link to="/group/new" >New group</Link>

                    </Menu.Item>
                </SubMenu>
        }
        return (
            <Layout style={{ minHeight: '100vh' }}>
                <Sider collapsible collapsed={this.state.collapsed} onCollapse={this.onCollapse}>
                    <div className="logo" />
                    <Menu theme="dark" defaultSelectedKeys={['all_students']} mode="inline">
                        {isLogout}
                        {Students}
                        {Groups}
                        <Menu.Item key="about">
                            {/* <FileOutlined /> */}
                            <Link to="/about">About Student Portal</Link>
                        </Menu.Item>
                    </Menu>
                </Sider>
                <Layout className="site-layout">
                    <Header className="site-layout-background" style={{ padding: 0 }} />
                    <Content style={{ margin: '0 16px' }}>
                        <Switch>
                            <Routing />
                        </Switch>
                    </Content>
                    <Footer style={{ textAlign: 'center' }}>Student Portal ©2018 Created by Pavel Pavlov</Footer>
                </Layout>
            </Layout >

        );
    }
}

export default LayoutStudentPortal;