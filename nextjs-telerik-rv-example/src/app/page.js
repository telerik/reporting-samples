"use client"

import dynamic from 'next/dynamic'

const TelerikReportViewer = dynamic(() => import('@progress/telerik-react-report-viewer')
                          .then(types => types.TelerikReportViewer), { ssr: false })

export default function Home() {
  return (
    <>
        <link href="https://kendo.cdn.telerik.com/themes/12.0.0/classic/classic-opal.css" rel="stylesheet" />

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
