import React, { Component } from 'react';
import './App.css';
import ViewerComponent from "./components/ViewerComponent";
import GridComponent from "./components/GridComponent";
import { speakersData } from "./data/speakers";
import { process } from '@progress/kendo-data-query';

const initialDataState = {
  sort: [
      { field: "Name", dir: "asc" }
  ],
}

class App extends Component {

  state = {
    data: process(speakersData,initialDataState),
    dataState: initialDataState
  }

  // Update the state based on the Grid component filter
  handleDataStateChange = (dataState) => {
    this.setState({
      data: process(speakersData, dataState),
      dataState: dataState
    })
  }
 
  render() {
    return (
      <div>
        <h1>Telerik Reporting & Kendo React Demo</h1>
          <div label="React Report Viewer">
            <div>
            <ViewerComponent data={this.state.data} dataState={this.state.dataState}></ViewerComponent>
            </div>
          </div>
          <div label="Kendo React Data Grid">
          <GridComponent data={this.state.data} handleDataStateChange={this.handleDataStateChange} dataState={this.state.dataState}></GridComponent>
          </div>
      </div>
    );
  }
}

export default App;
