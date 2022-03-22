import React, { Component } from 'react';
import './App.css';
   
import ReportDesigner from './components/ReportDesigner';

class App extends Component {
  render() {
    return (
      <div className="App">
        <ReportDesigner />
      </div>
    );
  }
}

export default App;
