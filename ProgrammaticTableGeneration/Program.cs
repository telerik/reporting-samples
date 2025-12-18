namespace ProgrammaticTableGeneration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // make sure to adjust the connection string and select command to match your database
            string selectCommand = "SELECT * FROM production.productphoto;";
            string connectionString = "server=localhost\\sqlexpress;database=AdventureWorks2022;trusted_connection=true;";

            // create the report
            Telerik.Reporting.Report report = new Telerik.Reporting.Report();
            report.Name = "Report1";
            report.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D));
            report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            report.Width = Telerik.Reporting.Drawing.Unit.Cm(17D);

            // add the main sections to the report
            var pageHeaderSection = new Telerik.Reporting.PageHeaderSection();
            pageHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5D);
            pageHeaderSection.Name = "pageHeaderSection";

            var detailSection = new Telerik.Reporting.DetailSection();
            detailSection.Height = Telerik.Reporting.Drawing.Unit.Cm(5D);
            detailSection.Name = "detail";

            var pageFooterSection = new Telerik.Reporting.PageFooterSection();
            pageFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5D);
            pageFooterSection.Name = "pageFooterSection";

            report.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { pageHeaderSection, detailSection, pageFooterSection });

            // prepare the data source for the table
            var sqlDataSource = new Telerik.Reporting.SqlDataSource(connectionString, selectCommand);
            sqlDataSource.Name = "sqlDataSource";
            sqlDataSource.ProviderName = "System.Data.SqlClient";

            // create the table and set its data source
            var table = new Telerik.Reporting.Table();
            table.Name = "table";
            table.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.5D), Telerik.Reporting.Drawing.Unit.Cm(1.5D));
            table.DataSource = sqlDataSource;
            detailSection.Items.Add(table);

            // add a detail row group to the table
            var rowGroup = new Telerik.Reporting.TableGroup();
            rowGroup.Name = "detailTableGroup";
            rowGroup.Groupings.Add(new Telerik.Reporting.Grouping(null));
            table.RowGroups.Add(rowGroup);
            table.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.609D)));

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                // retrieve the column names from the data source
                var command = new System.Data.SqlClient.SqlCommand(selectCommand, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);
                var columnNames = dataTable.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName);

                // for each column name
                int colIndex = 0;
                foreach (string columnName in columnNames)
                {
                    // add column
                    var column = new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2.99D));
                    table.Body.Columns.Add(column);

                    // add column header
                    var headerTextBox = new Telerik.Reporting.TextBox();
                    headerTextBox.Name = columnName + "TextBoxHeader";
                    headerTextBox.Value = columnName;
                    headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.99D), Telerik.Reporting.Drawing.Unit.Cm(0.609D));

                    var columnGroup = new Telerik.Reporting.TableGroup();
                    columnGroup.Name = columnName;
                    columnGroup.ReportItem = headerTextBox;
                    table.ColumnGroups.Add(columnGroup);

                    // add column body
                    var bodyTextBox = new Telerik.Reporting.TextBox();
                    bodyTextBox.Name = columnName + "TextBoxBody";
                    bodyTextBox.Value = "= Fields." + columnName;
                    bodyTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.99D), Telerik.Reporting.Drawing.Unit.Cm(0.609D));
                    table.Body.SetCellContent(0, colIndex, bodyTextBox);

                    colIndex++;
                }
            }

            // package the programmatically created report into a .trdp file
            var reportPackager = new Telerik.Reporting.ReportPackager();
            using (var targetStream = System.IO.File.Create("PackageReport1.trdp"))
            {
                reportPackager.Package(report, targetStream);
            }
            System.Console.WriteLine("Report created successfully at: " + Path.GetFullPath("PackageReport1.trdp"));
        }
    }
}
