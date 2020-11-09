import { AfterViewInit, Component, ViewChild  } from '@angular/core';
import { TelerikReportViewerComponent } from '@progress/telerik-angular-report-viewer';
import { StringResources } from './stringResources';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html'
})

export class HomeComponent implements AfterViewInit {
    @ViewChild('viewer1', { static: false }) viewer: TelerikReportViewerComponent;

    ngAfterViewInit(): void {
        // Localization demo.
        const language = navigator.language;
        let resources = StringResources.english; // Default.

        if (language === 'ja-JP') {
            resources = StringResources.japanese;
        }

        this.viewer.viewerObject.stringResources = Object.assign(this.viewer.viewerObject.stringResources, resources);
    }

    title = "Report Viewer";
    viewerContainerStyle = {
        position: 'absolute',
        left: '5px',
        right: '5px',
        top: '0',
        bottom: '5px',
        overflow: 'hidden',
        clear: 'both',
        ['font-family']: 'ms sans serif'
    };

    ready() { console.log('ready'); }
    viewerToolTipOpening(e: any, args: any) { console.log('viewerToolTipOpening ' + args.toolTip.text); }
}
