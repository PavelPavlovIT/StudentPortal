import React, { Component } from 'react';

import './App.css';
import LayoutStudentPortal from './components/containers/LayoutStudentPortal';
import { Router } from 'react-router-dom';
import history from './service/history';
import { getWebApi } from './service/auth';

class App extends Component {

  constructor(props: Readonly<{}>) {
    super(props)
    let url = getWebApi();

  }
  componentDidMount() {
    
  }

  render() {
    return (
      <Router history={history}>
        <LayoutStudentPortal />
      </Router>
    );
  }
}

export default App;