import { Component, OnInit, Input, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { TelerikReportViewerComponent } from '@progress/telerik-angular-report-viewer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'my-app';
  
  selectedTab: number;
  reportServerUrl: string;
  viewerContainerStyle: any;

  @ViewChild('rptViewer1', {static: false}) rptViewer1: TelerikReportViewerComponent;
  @ViewChild('rptViewer2', {static: false}) rptViewer2: TelerikReportViewerComponent;

  constructor() {
    //super();
      
    this.reportServerUrl = 'http://localhost:12345/api/reports';
    this.viewerContainerStyle = {
      "position": 'relative',
      "width": '1000px',
      "height": '1500px',
      ['font-family']: 'ms sans serif'
    };
  } 


  onLoad1Click(event) {
 
    var reportSource = {
      report: "BarcodesReport.trdp", 
    };

    if (this.rptViewer1) {
      this.rptViewer1.setReportSource(reportSource);
    }  
  }

  onLoad2Click(event) {
 
    var reportSource = {
        report: "BarcodesReport.trdp", 
    };

    if (this.rptViewer2) {
      this.rptViewer2.setReportSource(reportSource);
    }
  }
}
