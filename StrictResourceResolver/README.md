# Strict Resource Resolver Sample

This sample implements `Telerik.Reporting.Interfaces.IStrictResourceResolver` in a .NET Standard 2.0 library. The intended Windows consumers are .NET 10 and .NET Framework 4.6.2 applications.

The separate `ViewerService` project hosts an ASP.NET Core Reporting REST service and an HTML5 Report Viewer. It references the resolver library directly and does not depend on a clone of the Reporting source repository.

## Resource Policy

`TrustedResourceResolver` permits these resources:

- Files beneath one configured absolute local directory, including relative paths, absolute paths, and local file URIs.
- PNG images directly in the HTTPS folder supplied through the `trustedImageUrl` constructor argument. The examples use `https://raw.githubusercontent.com/telerik/reporting-docs/master/report-items/images/`, which contains both `Shape.png` and `Textbox.png`. Subfolders are not approved.

Other URLs, UNC/device paths, alternate data streams, paths outside the directory, and symbolic links or junctions are denied. Policy denials return `null` and emit a trace warning. Missing files, invalid configuration, and download failures raise exceptions.

Approved resources return bytes, not locations. The public PNG download has a 15-second cancellation deadline and a 5 MiB limit. The downloader checks the media type and PNG signature, disables redirects and cookies, and sends no credentials. These checks do not replace full image validation.

The public URL refers to a mutable branch. Verify its availability, then replace it with a commit-pinned URL if reproducible content is required. Do not allow all of `raw.githubusercontent.com`: that host also serves unrelated repositories.

This interface receives a resource identifier, not the report-item type. An approved image URL is approved whenever a consumer requests it; the sample cannot restrict that URL exclusively to `PictureBox`.

The file policy does not automatically inherit `validateReportPaths` or `externalReportPaths`. Keep report-path validation enabled and adapt the resource policy to your application. Arbitrary map tile URLs remain denied.

Keep the resource tree and its ancestors under administrator control. Linked-component checks occur before the file read and do not prevent concurrent filesystem replacement. Do not treat the sample as a comprehensive policy for a tree that untrusted users can modify.

## Prerequisites

Use a .NET SDK that supports .NET 10 and configure the [Telerik private NuGet feed](https://www.telerik.com/products/reporting/documentation/getting-started/installation/adding-private-nuget-feed). The Framework checks also require Windows, the .NET Framework 4.6.2 targeting pack, and a compatible runtime.

Use the same Reporting release for the library and both hosts. Configure licensing and the host's normal Reporting runtime dependencies. Do not copy the .NET Standard Reporting DLL over the Framework host's Reporting DLL.

The projects use Telerik Reporting NuGet packages version `20.3.26.1105`, which includes `IStrictResourceResolver`. Update all package references together if you use another release. NuGet selects the assets for each target framework and restores the package dependencies. Keep feed credentials outside the repository.

## Building

Open `StrictResourceResolver.sln` in Visual Studio. The solution contains two sibling projects:

- `StrictResourceResolver\StrictResourceResolver.csproj`: The reusable .NET Standard 2.0 class library.
- `ViewerService\ViewerService.csproj`: The .NET 10 REST service and HTML5 viewer, with a project reference to the library.

`Tests` remains a separate console validation project outside the solution.

From this folder, build both solution projects:

```PowerShell
dotnet build .\StrictResourceResolver.sln -c Release
```

The reusable library is `StrictResourceResolver\bin\Release\netstandard2.0\StrictResourceResolver.dll`.

## Running the Viewer and REST Service

Run the separate .NET 10 web project from this folder:

```PowerShell
dotnet run --project .\ViewerService\ViewerService.csproj
```

Open `http://localhost:5087`. The HTML5 viewer calls the service at `/api/reports` on the same host and opens `TrustedImage.trdp`.

The service uses the built-in `UriReportSourceResolver` with the local `ViewerService\Reports` folder. No custom report-source resolver is required. Open `Reports\TrustedImage.trdp` in the Standalone Report Designer to edit the report.

The report uses an embedded CSV data source with four labeled rows. Its `trustedImage` PictureBox binds to `Fields.ImageUrl`:

- **Allowed local image** uses `local-image.png` from the configured `Resources` directory.
- **Allowed trusted HTTPS image** uses `https://raw.githubusercontent.com/telerik/reporting-docs/master/report-items/images/Shape.png`, which is directly in the configured trusted folder.
- **Denied existing HTTPS image** uses `https://raw.githubusercontent.com/telerik/reporting-docs/master/report-items/graph/images/DataItems/Graph/BarLineChartStructure.png`. This PNG exists but is not approved, so its row displays without an image.
- **Denied existing local image** uses `..\DeniedResources\not-allowed-image.png`. This file exists in a sibling folder outside the approved `Resources` directory, so its row displays without an image. Both folders copy to build and publish output; only `Resources` is allowed.

Use direct image URLs. GitHub `blob` and `tree` pages return HTML rather than image bytes. The local value must identify a file, not just its directory.

`ViewerService\appsettings.json` registers the resolver, its trusted type, and both constructor arguments. The host converts the relative `Resources` directory to an absolute path before it supplies the configuration to Reporting. Change `directory` and `trustedImageUrl` to adapt the sample.

The viewer needs internet access for jQuery and the Kendo UI theme. The report image also needs internet access. The TRDP copies to build and publish output. The service supports the built-in HTML and PDF renderers; additional export formats can require additional Reporting dependencies.

## Registering

Deploy `StrictResourceResolver.dll` beside the host's application assemblies. Create a resource directory and grant the host read access.

Merge `Examples\app.config` into a Framework application's `app.config` or `web.config`. Merge `Examples\appsettings.json` into a .NET application's Reporting configuration. Replace `directory` and `trustedImageUrl` with your approved locations.

Both examples use the existing `custom` provider and this assembly-qualified type:

```TEXT
StrictResourceResolver.TrustedResourceResolver, StrictResourceResolver
```

The examples also register the custom type in `typeReferences`, without disabling type validation.

The `directory` and `trustedImageUrl` parameters supply the public constructor's two string arguments. The provider sorts parameter names: `directory` first, then `trustedImageUrl`.

Supply an absolute HTTPS folder URL that ends in `/`, without credentials, query strings, fragments, or a nondefault port. Use the raw image endpoint, not a GitHub `tree` or `blob` page. Only direct files in that folder are approved; other hosts, sibling folders, subfolders, and traversal paths are denied. Requested URLs must also omit credentials, query strings, fragments, and nondefault ports.

No new provider or processing switch is required. Restart the application after deployment/configuration changes.

Set a report's image value to a relative file such as `image.png`, an approved absolute file location, or a direct PNG URL in the configured HTTPS folder. Inline image content does not require a new external-resource lookup.

For direct construction, pass both approved locations:

```CSharp
var resolver = new StrictResourceResolver.TrustedResourceResolver(
    @"C:\ReportingResources",
    "https://raw.githubusercontent.com/telerik/reporting-docs/master/report-items/images/");
```

## Running Checks

`Tests` is a console validation project, separate from the viewer application. It uses the same `netstandard2.0` library for both targets.

Run the .NET 10 checks:

```PowerShell
dotnet run --project .\Tests\StrictResourceResolver.Tests.csproj -c Release -f net10.0
```

Build and execute the Framework checks:

```PowerShell
dotnet build .\Tests\StrictResourceResolver.Tests.csproj -c Release -f net462
.\Tests\bin\Release\net462\StrictResourceResolver.Tests.exe
```

Checks cover allowed file bytes, directory boundaries, invalid folder configuration, multiple images in the trusted HTTPS folder, URL boundaries, traversal/subfolder denial, approved download bytes, redirect/error responses, invalid PNG responses, and oversized content without a declared length. Fake handlers keep these checks independent of public services.

Run the host-specific executable with `--public-image` to check the real URL. Run it with `--integration` to check Reporting configuration and image rendering. For .NET 10, append the flags after `--` in `dotnet run`. For Framework, append them to the executable command.

Integration checks temporarily update and restore the validation executable's own configuration, not your application configuration. Run them only in the dedicated validation output directory. The checks verify approved relative image rendering and a trace denial from the configured resolver for an outside image.

Validation completed on Windows with Telerik Reporting `20.3.26.1105`: Release solution build, Chromium loading of the local `TrustedImage.trdp` through the REST service, and successful decoding of the trusted `Shape.png` image. Earlier console checks with `--public-image --integration` passed for both `net10.0` and `net462`. The Framework executable ran on the installed Windows Framework runtime, not a separate clean installation of .NET Framework 4.6.2.

NuGet emitted `NU1900` because it could not retrieve vulnerability data from the Telerik feed. Package restore, compilation, and runtime checks succeeded.

Before deployment, extend validation for your policy to include stalled-response cancellation, linked paths/junctions, inaccessible files, and any additional resource consumers you use. Test the production HTTP handler's redirect behavior with a controlled endpoint. The completed image checks do not establish behavior for every resource consumer.
