# Telerik Angular Viewer and Web Report Designer

This sample demonstrates how to host the Telerik Reporting HTML5 Report Viewer and Web Report Designer in an Angular application. The application loads the Telerik jQuery widgets at runtime and provides a navigation link for each component:

- **Designer** at `/` opens `Business/Dashboard.trdx`.
- **Viewer** at `/viewer` opens `Report Catalog.trdx` in print-preview mode.

By default, both components use the public Telerik Reporting demo services. An internet connection is required when running the sample.

## Prerequisites

- Node.js and npm
- Access to the Telerik Reporting demo services, or a running Telerik Reporting REST service if you are switching to a local backend

## Install and run

Install the dependencies and start the Angular development server:

```bash
npm install
npm start
```

Open <http://localhost:4200/> and choose **Designer** or **Viewer** from the navigation bar. The theme switcher changes the Kendo UI Meridian theme between light and dark mode.

## Available commands

| Command | Description |
| --- | --- |
| `npm start` | Start the development server at `http://localhost:4200/`. |
| `npm run build` | Create a production build in `dist/telerik-angular-viewer-and-designer/`. |
| `npm run watch` | Rebuild the application automatically when files change. |

## Configure a local Reporting service

The service URLs and report names are defined in the component classes:

- [viewer.component.ts](src/app/viewer/viewer.component.ts) uses `https://demos.telerik.com/reporting/api/reports/`.
- [designer.component.ts](src/app/designer/designer.component.ts) uses `https://demos.telerik.com/reporting/api/reportdesigner/`.

Replace those URLs with the corresponding endpoints from your Telerik Reporting REST service. The designer component includes a local-service example in its source. Update the report identifiers as needed for the reports available to your service.

The Telerik scripts are loaded from the Reporting CDN by [script-loader.ts](src/app/script-loader.ts) and the viewer/designer components. If your deployment cannot access the CDN, host compatible script versions locally and update the script URLs in those components.

## Project structure

```text
src/app/
	designer/       Web Report Designer component
	viewer/         HTML5 Report Viewer component
	nav-menu/       Navigation and theme switcher
	script-loader.ts Runtime script loader used by the Reporting widgets
```
