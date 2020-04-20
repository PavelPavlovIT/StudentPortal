import React from 'react';
import { Link } from 'react-router-dom';
import { Menu } from 'antd';

export default class _Header extends React.Component {
    render() {
        return (
            <Menu mode="horizontal">
                <Menu.Item key="mail">
                    <Link to="/">Студенты</Link>
                </Menu.Item>
                <Menu.Item key="app" >
                    <Link to="/about">О программе</Link>
                </Menu.Item>
            </Menu >
        );
    }
};