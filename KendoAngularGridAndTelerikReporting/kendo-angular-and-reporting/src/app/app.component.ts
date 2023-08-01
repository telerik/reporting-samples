import { Component } from '@angular/core';
import { ViewChild } from '@angular/core';
import { filterBy, CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { speakersData } from '../data/speakers';
import { ReportViewerComponent } from '../app/report-viewer/report-viewer.component'

const distinctCategories = data => data
    .filter((x, idx, xs) => xs.findIndex(y => y.DaySpeaking === x.DaySpeaking) === idx);

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent {
    title = 'app';

    @ViewChild('reportViewer1', {}) viewer: ReportViewerComponent;

    public get telerikReportViewer() {
        return this.viewer.viewer;
    }
    public filter: CompositeFilterDescriptor = null;
    public gridData: any[] = filterBy(speakersData, this.filter);
    public categories: any[] = distinctCategories(speakersData);

    public setViewerData(data) {
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
