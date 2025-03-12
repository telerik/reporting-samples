namespace CrystalReportsConverter
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            const string crystalReportsFolder = @"c:\crystal-reports";
            new CrystalReportsConverter.Converter()
                .Convert(crystalReportsFolder);
        }
    }
}
