import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Trade } from './components/Trade';
import { History } from './components/History';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Trade} />
        <Route path='/trade' component={Trade} />
        <Route path='/history' component={History} />
      </Layout>
    );
  }
}
