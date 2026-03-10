"use client"

import dynamic from 'next/dynamic'

const TelerikReportViewer = dynamic(() => import('@progress/telerik-react-report-viewer')
                          .then(types => types.TelerikReportViewer), { ssr: false })

export default function Home() {
  return (
    <>
        <link href="https://kendo.cdn.telerik.com/themes/12.3.0/classic/classic-opal.css" rel="stylesheet" />

      <TelerikReportViewer
        serviceUrl="http://localhost:5292/api/reports/"
        reportSource={{
            report: 'SampleReport.trdp',
            parameters: {}
        }}
        viewerContainerStyle = {{
            position: 'absolute',
            inset: '100px'
        }}
        viewMode="INTERACTIVE"
        scaleMode="SPECIFIC"
        scale={1.0}
        enableAccessibility={false} />
    </>
  );
}
