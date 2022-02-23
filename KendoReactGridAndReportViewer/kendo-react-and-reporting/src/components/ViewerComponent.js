import React, { Component } from "react";
import { TelerikReportViewer } from '@progress/telerik-react-report-viewer'

class ViewerComponent extends Component {
    viewer;

    // make a request each time the data is changed
    componentDidUpdate() {
        let reportdata = JSON.stringify(this.props.data.data);
        let rs = {
            report: 'Speakers Report.trdp',
            parameters: { DataParameter: reportdata }
        };
        this.viewer.setReportSource(rs);
    }

    render() {

        let speakerObjects = JSON.stringify(this.props.data.data);
        return (
            <TelerikReportViewer
                ref={el => this.viewer = el}

                serviceUrl="http://localhost:59655/api/reports/"
                reportSource={{
                    report: 'Speakers Report.trdp',
                    parameters: {
                        'DataParameter': speakerObjects
                    }
                }}
                viewerContainerStyle={{
                    position: 'absolute',
                    height: '90%',
                    width: '55%',
                    top: '6%',
                    overflow: 'hidden',
                    clear: 'both',
                    fontFamily: 'ms sans serif'
                }}
                scaleMode="SPECIFIC"
                scale={1.2}
                enableAccessibility={false} />
        );
    }
}

export default ViewerComponent;
