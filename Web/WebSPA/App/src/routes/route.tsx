import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import About from '../components/containers/about';

import ItemStudent from '../components/itemStudent';
import GroupItem from '../components/GroupItem';
import GroupList from '../components/GroupList';
import { Student } from '../components/models/student';
import ListStudents from '../components/listStudents';
import Login from '../components/login';
import Registration from '../components/registration';


interface IProps { }
interface IState {
    students: Student[];
}

export default class Routing extends React.Component<IProps, IState> {
    subscription: any;
    constructor(props: Readonly<{}>) {
        super(props)
        this.state = {
            students: []
        };
    }

    componentDidMount() {
    }

    render() {
        return (
            <main>
                <Switch>
                    <Route path="/registration" component={Registration} />
                    <Route path="/login" component={Login} />
                    <Route path="/about" component={About} />
                    <Route path="/students" component={ListStudents} />
                    <Route path="/mystudents" component={ListStudents} />
                    <Route path="/student/new" component={ItemStudent} />
                    <Route path="/student/modify" component={ItemStudent} />
                    <Route path="/group/all" component={GroupList} />
                    <Route path="/group/new" component={GroupItem} />
                    <Route path="/group/update" component={GroupItem} />
                    <Route exact path="/" render={() => (<Redirect to="/students" />)} />
                </Switch>
            </main>
        );
    }
};
