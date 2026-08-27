import { Component, OnInit } from '@angular/core';
import { loadScript } from '../script-loader';

declare const $: any;

@Component({
  selector: 'app-viewer',
  templateUrl: './viewer.component.html',
  styleUrls: ['./viewer.component.css']
})
export class ViewerComponent implements OnInit {

  private readonly serviceUrl = 'https://demos.telerik.com/reporting/api/reports/';

  private viewer: any;

  ngOnInit(): void {
    this.loadScripts().then(() => {
      this.viewer = $("#reportViewer").telerik_ReportViewer({
        serviceUrl: this.serviceUrl,
        reportSource: {
          report: 'Report Catalog.trdx',
          parameters: {}
        },
        viewMode: 'PRINT_PREVIEW',
      }).data("telerik_ReportViewer");
    });
  }

  private loadScripts(): Promise<void> {
    const scripts = [
      'https://code.jquery.com/jquery-3.7.1.min.js',
      'https://reporting.cdn.telerik.com/20.2.26.812/js/webReportDesigner.kendo.min.js',
      'https://reporting.cdn.telerik.com/20.2.26.812/js/telerikReportViewer.min.js'
    ];

    let chain = Promise.resolve();
    for (const src of scripts) {
      chain = chain.then(() => loadScript(src));
    }
    return chain;
  }
}

