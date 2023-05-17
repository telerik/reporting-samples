# Telerik Reporting Samples Repository

Welcome to the GitHub repository for [Progress Telerik Reporting](https://www.telerik.com/reporting) sample projects. This repository contains a vast range of projects that
have been created over the years for blogs, articles, videos, etc. related to the Telerik Reporting product.

There are lots of projects in this repository that target an older version of Telerik Reporting because they have been created to solve a specific problem and to provide a 
workaround/solution to a breaking change for a specific version of the product - [Telerik Reporting Release History](https://www.telerik.com/support/whats-new/reporting/release-history).
For the above reason, most of the projects in this repository will *not* be kept up to date with regards to the used Telerik Reporting or .NET version.

## Nuget Package Manager
Before attempting to run any of the projects, it is recommended to set up the Telerik private NuGet feed as described in the [Adding the Telerik Private NuGet Feed to Visual Studio](https://docs.telerik.com/reporting/getting-started/installation/adding-private-nuget-feed) article.

## Trial / Dev Nuget Packages
The projects in this repository use either the `Trial` or `Dev` Telerik Reporting NuGet packages. With that being said, a `Trial` user should be able to use any project
created with `Dev` packages as long as the `.Trial` suffix is added to the end of the package name, the reverse is also true(removing the suffix).

For example, if the `DEV` name of a Telerik Reporting package is `Telerik.Reporting`, change it to `Telerik.Reporting.Trial` in the `.csproj` file of the given project.  

## Up-to-date Telerik Reporting Examples

For those that wish to experience the latest of Telerik Reporting, we recommend having a look at the *Integration* demo projects shipped with the installation of the product.

For example, with a default installation, the path to the folder with the examples will be:  `C:\Program Files (x86)\Progress\Telerik Reporting R1 2023\Examples`
