import { Component, ViewChild } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { TelerikReportViewerComponent, TelerikReportingModule } from '@progress/telerik-angular-report-viewer';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ MatTabsModule, MatButtonModule, TelerikReportingModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent {
  //@ts-ignore
  @ViewChild('rptViewer1', { static: false }) rptViewer1: TelerikReportViewerComponent;
  //@ts-ignore
  @ViewChild('rptViewer2', { static: false }) rptViewer2: TelerikReportViewerComponent;

  title = 'my-app';
  selectedTab: number = 0;
  reportServerUrl: string;
  viewerContainerStyle: any;

  constructor() {
    this.reportServerUrl = 'http://localhost:5292/api/reports/';
    this.viewerContainerStyle = {
      "position": 'relative',
      "width": '75vw',
      "height": '85vh',
      ['font-family']: 'ms sans serif'
    };
  }

  onLoad1Click() {

    var reportSource = {
      report: "SampleReport.trdp",
    };

    if (this.rptViewer1) {
      this.rptViewer1.setReportSource(reportSource);
    }
  }

  onLoad2Click() {

    var reportSource = {
      report: "SampleReport.trdp",
    };

    if (this.rptViewer2) {
      this.rptViewer2.setReportSource(reportSource);
    }
  }
}
