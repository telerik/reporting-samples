import React, { Component } from "react";
import { Grid, GridColumn } from '@progress/kendo-react-grid';
import { MultiSelect } from '@progress/kendo-react-dropdowns';
import { categories } from "../data/categories";

class GridComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            dropdownlistCategory: null,
            gridClickedRow: {}
        }
    }
    handleMultiSelectChange = (e) => {
        let newDataState = { ...this.props.dataState }

        if (e.value.length > 0) {
            let expressions = [];
            e.value.forEach((item) => {
                console.log(item)
                expressions.push({ field: 'CategoryID', operator: 'eq', value: item.CategoryID });
            });

            newDataState.filter = {
                logic: 'or',
                filters: expressions
            }
            newDataState.skip = 0

        } else {
            newDataState.filter = []
            newDataState.skip = 0
        }
        this.setState({
            dropdownlistCategory: e.target.value.CategoryID
        });

        //Update the parent component
        this.props.handleDataStateChange(newDataState)
    }

    handleGridDataStateChange = (e) => {
        //Update the parent component
        this.props.handleDataStateChange(e.dataState)
    }

    handleGridRowClick = (e) => {
        this.setState({
            gridClickedRow: e.dataItem
        });
    }

    render() {
        return (
            <div>
                <MultiSelect
                    data={categories}
                    dataItemKey="CategoryID"
                    textField="CategoryName"
                    defaultValue={categories}
                    style={{ width: "22%", left: "57%" }}
                    onChange={this.handleMultiSelectChange}
                />

                <Grid
                    data={this.props.data}
                    {...this.props.dataState}
                    onDataStateChange={this.handleGridDataStateChange}
                    style={{ height: "50%", width: "30%", left: "57%", top: "10%" }}>
                    <GridColumn
                        field="ImageUrl"
                        title="Photo"
                        width="75%"
                        cell={props => (
                            <td>
                                <img
                                    style={{ width: 50, height: 50 }}
                                    src={props.dataItem.ImageUrl}
                                />
                            </td>
                        )}
                    />
                    <GridColumn field="Name" title="Name" width="150%" />
                    <GridColumn field="Company" title="Company" width="130%" />
                    <GridColumn field="Position" title="Position" width="220%" />
                    <GridColumn field="DaySpeaking" title="Day Speaking" width="123%" />
                </Grid>
            </div>
        );
    }
}

export default GridComponent;
