namespace tr_dpi_detector
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using Processing = Telerik.Reporting.Processing;

    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(string.Concat("Rendered ", Program.RenderReport()));
                Console.Write("Press a key to render again or ESC to break.");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        private static Report CreateReport()
        {
            Report report = new Report();
            DetailSection detail = new DetailSection()
            {
                Height = Unit.Cm(4)
            };
            TextBox textBox1 = new TextBox()
            {
                Width = Unit.Cm(14),
                Height = Unit.Cm(2),
                Left = Unit.Cm(1),
                Top = Unit.Cm(1),
                Value = string.Concat("DPI: ", Program.GetDpi().ToString())
            };
            
            textBox1.Style.Font.Size = Unit.Point(36);
            textBox1.Style.TextAlign = HorizontalAlign.Center;
            
            detail.Items.Add(textBox1);
            report.Items.Add(detail);

            return report;
        }

        private static float GetDpi()
        {
            float horizontalResolution;
            using (Bitmap bmp = new Bitmap(2, 2))
            {
                horizontalResolution = bmp.HorizontalResolution;
            }
            return horizontalResolution;
        }

        private static string RenderReport()
        {
            var reportProcessor = new Processing.ReportProcessor();
            var result = reportProcessor.RenderReport("IMAGE", new InstanceReportSource()
            {
                ReportDocument = Program.CreateReport()
            }, new Hashtable()
            {
                { "OutputFormat", "PNG" }
            });
            string timeStamp = DateTime.Now.ToString("HHmmss");
            string fileName = string.Concat("report_", timeStamp, ".png");
            File.WriteAllBytes(fileName, result.DocumentBytes);

            return fileName;
        }
    }
}
