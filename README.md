# reporting-samples
This branch contains a project that detects the machine's DPI.

The project is a .NET Framework 4.8 console application. 

**WHAT IT DOES?**

It dynamically creates a Report instance and adds a single TextBox in it.
The value of the TextBox contains the machine's DPI, as detected through the Bitmap class, and also the machine's DPI, as detected by Telerik.Reporting.Drawing.Unit class.
The output looks like this:

**DPI (from Bitmap): 96;**

**DPI (from Unit): 96**


The report is rendered to a PNG image.

**WORKFLOW**

The console application renders the report once and waits for a key press to render again. Pressing ESC exits the application.
