import React, { Component } from 'react';

export default class ReportDesigner extends Component {

    componentDidMount() {
        window.jQuery('#reportDesigner1')
            .telerik_WebReportDesigner({
                toolboxArea: {
                    layout: "list" //Change to "grid" to display the contents of the Components area in a flow grid layout.
                },
                serviceUrl: "https://demos.telerik.com/reporting/api/reportdesigner/",
                report: "Barcodes report.trdx"
            }).data("telerik_WebDesigner");
    }

    render() {
        return <div id="reportDesigner1"></div>
    }
}

