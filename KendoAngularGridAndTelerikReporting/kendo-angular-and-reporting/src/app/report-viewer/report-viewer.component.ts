// import { AfterViewInit, Component, ViewChild } from '@angular/core';
// import { TelerikReportViewerComponent } from '@progress/telerik-angular-report-viewer';
// import { StringResources } from './stringResources';

// @Component({
//     selector: 'report-viewer',
//     templateUrl: './report-viewer.component.html',
//     styleUrls: ['./report-viewer.component.css']
// })

// export class ReportViewerComponent implements AfterViewInit {
//     @ViewChild('viewer1') viewer: TelerikReportViewerComponent;

//     ngAfterViewInit(): void {
//         // Localization demo.
//         const language = navigator.language;
//         let resources = StringResources.english; // Default.

//         if (language === 'ja-JP') {
//             resources = StringResources.japanese;
//         }

//         this.viewer.viewerObject.stringResources = Object.assign(this.viewer.viewerObject.stringResources, resources);
//     }

//     title = "Report Viewer";
//     viewerContainerStyle = {
//         position: 'absolute',
//         left: '5px',
//         right: '5px',
//         top: '40px',
//         bottom: '5px',
//         overflow: 'hidden',
//         clear: 'both',
//         ['font-family']: 'ms sans serif'
//     };

//     ready() {
//         console.log('ready');
//     }
//     viewerToolTipOpening(e: any, args: any) {
//         console.log('viewerToolTipOpening ' + args.toolTip.text);
//     }
// }

import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { TelerikReportViewerComponent } from '@progress/telerik-angular-report-viewer';
import { StringResources } from './stringResources';
import { speakersData } from '../../data/speakers';

@Component({
    selector: 'report-viewer',
    templateUrl: './report-viewer.component.html',
    styleUrls: ['./report-viewer.component.css']
})

export class ReportViewerComponent implements AfterViewInit {

    
    @ViewChild('viewer1', {}) viewer: TelerikReportViewerComponent;

    speakerObjects = JSON.stringify(speakersData);


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
        width: '100%',
        height: '980px',
        ['font-family']: 'ms sans serif'
    };

    ready() {
        console.log('ready');
    }
    viewerToolTipOpening(e: any, args: any) {
        console.log('viewerToolTipOpening ' + args.toolTip.text);
    }
 
}

