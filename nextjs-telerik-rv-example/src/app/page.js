"use client"

import { TelerikReportViewer } from '@progress/telerik-react-report-viewer'

export default function Home() {
  return (
    <>
        <link href="https://kendo.cdn.telerik.com/2022.3.913/styles/kendo.common.min.css" rel="stylesheet" />
        <link href="https://kendo.cdn.telerik.com/2022.3.913/styles/kendo.blueopal.min.css" rel="stylesheet" />

      <TelerikReportViewer
        serviceUrl="https://demos.telerik.com/reporting/api/reports/"
        reportSource={{
            report: 'Dashboard.trdx',
            parameters: {}
        }}
        viewerContainerStyle = {{
            position: 'absolute',
            inset: '5px'
        }}
        viewMode="INTERACTIVE"
        scaleMode="SPECIFIC"
        scale={1.0}
        enableAccessibility={false} />
    </>
  );
}
