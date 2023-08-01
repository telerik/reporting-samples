import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { TelerikReportingModule } from '@progress/telerik-angular-report-viewer';
import { ReportViewerComponent } from './report-viewer/report-viewer.component';


import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GridModule } from '@progress/kendo-angular-grid';


@NgModule({
  declarations: [
    AppComponent,
    ReportViewerComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    TelerikReportingModule,
    GridModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
