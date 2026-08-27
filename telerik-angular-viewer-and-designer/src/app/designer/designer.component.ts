import { Component, OnInit } from '@angular/core';
import { loadScript } from '../script-loader';

declare const $: any;

@Component({
  selector: 'app-designer',
  templateUrl: './designer.component.html',
  styleUrls: ['./designer.component.css']
})

export class DesignerComponent implements OnInit {
  // For local services, use, for example: http://localhost:5000/api/reportdesigner/
  private readonly serviceUrl = 'https://demos.telerik.com/reporting/api/reportdesigner/';

  private designer: any;

  ngOnInit(): void {
    this.loadScripts().then(() => {
      this.designer = $("#webReportDesigner").telerik_WebReportDesigner({
        serviceUrl: this.serviceUrl,
        useExternalTheme: true,
        report: 'Business/Dashboard.trdx'
      }).data("telerik_WebDesigner");
    });
  }

  private loadScripts(): Promise<void> {
    return loadScript('https://code.jquery.com/jquery-3.7.1.min.js')
      .then(() => loadScript('https://reporting.cdn.telerik.com/20.2.26.812/js/webReportDesigner.kendo.min.js'))
      .then(() => loadScript('https://reporting.cdn.telerik.com/20.2.26.812/js/telerikReportViewer.min.js'))
      .then(() => loadScript('https://reporting.cdn.telerik.com/20.2.26.812/js/webReportDesigner.min.js'));
  }
}
