import { Component, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GridModule } from '@progress/kendo-angular-grid';
import { filterBy, CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { speakersData } from '../data/speakers';
import { ReportViewerComponent } from './report-viewer/report-viewer.component';

const distinctCategories = (data: any[]) => data
  .filter((x, idx, xs) => xs.findIndex(y => y.DaySpeaking === x.DaySpeaking) === idx);

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GridModule, ReportViewerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'app';
  //@ts-ignore
  @ViewChild('reportViewer1', {}) viewer: ReportViewerComponent;

  public get telerikReportViewer() {
    return this.viewer.viewer;
  }
  //@ts-ignore
  public filter: CompositeFilterDescriptor = null;
  public gridData: any[] = filterBy(speakersData, this.filter);
  public categories: any[] = distinctCategories(speakersData);

  public setViewerData(data: string) {
    var rs = {
      report: this.telerikReportViewer.reportSource.report,
      parameters: { DataParameter: data }
    };
    this.telerikReportViewer.setReportSource(rs);
  }

  public filterChange(filter: CompositeFilterDescriptor): void {
    this.filter = filter;
    var filteredData = filterBy(speakersData, filter);
    this.gridData = filteredData;
    this.setViewerData(JSON.stringify(filteredData));
  }
}
