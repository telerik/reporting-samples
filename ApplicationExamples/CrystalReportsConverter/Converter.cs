namespace CrystalReportsConverter
{
    using System;
    using System.IO;
    using Telerik.Reporting.Interfaces;

    class Converter
    {
        public void Convert(string directory)
        {
            var logger = new ConsoleLogger();
            logger.LogInfo($"Conversion started in folder {directory}.");
            var files = Directory.GetFiles(directory, "*.rpt");
            try
            {                
                for (int i = 0; i < files.Length; i++)
                {
                    logger.LogInfo($"Converting file {i + 1}/{files.Length}: {Path.GetFileName(files[i])}...");
                    Convert(files[i], logger);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            finally
            {
                logger.LogInfo($"Conversion of {files.Length} Crystal Report files completed.");
            }
        }

        void Convert(string crystalReportFilePath, ILog logger)
        {
            var crConverter = new Telerik.ReportConverter.CrystalReports.CrystalReportsConverter();
            var trReport = crConverter.Convert(crystalReportFilePath, logger);
            
            var telerikReportFilePath = Path.ChangeExtension(crystalReportFilePath, "trdx");
            new Telerik.Reporting.XmlSerialization.ReportXmlSerializer()
                .Serialize(telerikReportFilePath, trReport);
        }
    }
}
