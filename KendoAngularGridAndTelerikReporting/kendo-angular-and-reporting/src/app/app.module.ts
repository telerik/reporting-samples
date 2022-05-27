import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GridModule } from '@progress/kendo-angular-grid';
import { MultiSelectModule } from '@progress/kendo-angular-dropdowns';
import { AppComponent } from './app.component';
import { MultiSelectWrapperComponent } from './kendo-multiselect/multiselect.component';
import { TelerikReportingModule } from '@progress/telerik-angular-report-viewer';
import { ReportViewerComponent } from './report-viewer/report-viewer.component';

@NgModule({
  bootstrap: [
    AppComponent
  ],
  declarations: [
    AppComponent,
    MultiSelectWrapperComponent,
    ReportViewerComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    GridModule,
    MultiSelectModule,
    TelerikReportingModule
  ]
})
export class AppModule { }
