import { Component } from '@angular/core';

declare var $: any;

@Component({
  selector: 'app-viewer',
  templateUrl: './viewer.component.html',
  styleUrls: ['./viewer.component.css']
})
export class ViewerComponent {

  viewer: null | undefined;

  ngOnInit(): void {
    this.viewer = $("#reportViewer").telerik_ReportViewer({
      serviceUrl: "https://demos.telerik.com/reporting/api/reports/",
      reportSource: {
        report: 'Report Catalog.trdx',
        parameters: {}
      }
    }).data("telerik_ReportViewer");
  }

}

