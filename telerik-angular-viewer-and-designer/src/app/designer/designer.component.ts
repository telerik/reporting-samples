import { Component } from '@angular/core';

declare var $: any;

@Component({
  selector: 'app-designer',
  templateUrl: './designer.component.html',
  styleUrls: ['./designer.component.css']
})

export class DesignerComponent {

  designer: any;
  
  ngOnInit(): void {
    this.designer = $("#webReportDesigner").telerik_WebReportDesigner({
      serviceUrl: "https://demos.telerik.com/reporting/api/reportdesigner/",
      report: "Product Catalog.trdx"
    }).data("telerik_WebDesigner");
  }

  ngOnDestroy(): void {
    var webReportDesignerTheme = $("link[href*='ext_styles/webReportDesignerTheme']");
    var webReportDesigner = $("link[href*='styles/webReportDesigner']");
    var fonticons = $("link[href*='font/fonticons']");

    //Telerik Web Report Designer loads all required styles when the widget is loaded.
    //In order to prevent them from dublication, remove the last instance  
    if (webReportDesignerTheme.length > 1) {
      webReportDesignerTheme.last().remove();
    }

    if (webReportDesigner.length > 1) {
      webReportDesigner.last().remove();
    }

    if (fonticons.length > 1) {
      fonticons.last().remove();
    }
  }
}
