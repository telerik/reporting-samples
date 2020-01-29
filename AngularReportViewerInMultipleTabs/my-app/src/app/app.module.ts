import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { DialogModule, /*ChartModule, OverlayPanelModule, TreeModule, ConfirmDialogModule*/ } from 'primeng/primeng';
import { MatTabsModule, /*MatIconModule, MatButtonModule, MatSidenavModule, MatInputModule, MatSelectModule, MatMenuModule, MatRadioModule, MatListModule, MatSlideToggleModule*/ } from "@angular/material";
import { TelerikReportingModule } from '@progress/telerik-angular-report-viewer';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,

    //DialogModule,
    MatTabsModule,
    TelerikReportingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
