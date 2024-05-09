// The collection of column names to be assigned to the Table
using System.Data;
using Telerik.Reporting;

//Initialization
string trdxFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "BlankReport.trdx");
string newTrdxFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "NewReport.trdx");
var myDataSource = GetData(5, 10);

IList<string> columnNames = new List<string>();
for (int colNumber = 0; colNumber < myDataSource.Columns.Count; colNumber++)
{
    string columnName = myDataSource.Columns[colNumber].ColumnName;
    columnNames.Add(columnName);
}

// Deserialize the initial report definition (assuming it is a .trdx file which path/name is stored in the variable trdxFileName)
Report report = DeserializeReport(trdxFileName);

// Get the Detail Section
var detailSection = (DetailSection)report.Items.FindItem(typeof(DetailSection));

// Get the Table from the Detail Section (assuming it is placed there)
var table = (Table)detailSection.Items.FindItem(typeof(Table));

// Assign DataSource to the Table. The DataSource should have all the properties from the columNames above
table.DataSource = myDataSource;

//Add columns to the Table
FormatTableBasedOnColumnNames(table, columnNames, true);

// Save updated report as .trdx file under newTrdxFileName
SerializeReport(report, newTrdxFileName);

RenderReportWithData(report);

void RenderReportWithData(Report report)
{
    var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
    var reportSource = new InstanceReportSource { ReportDocument = report };

    Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, null);

    if (!result.HasErrors)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "NewReport.pdf");

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
        }
    }
}

void FormatTableBasedOnColumnNames(Table table, IList<string> columnNames, bool addTotalRow)
{
    string tableHeaderStyleName = table.ColumnGroups[0].ReportItem.StyleName;
    string tableBodyStyleName = table.Items[0].StyleName;
    float columnnWidthValue = table.Width.Value / columnNames.Count();
    Telerik.Reporting.Drawing.Unit columnnWidth = new Telerik.Reporting.Drawing.Unit(columnnWidthValue, table.Width.Type);

    table.ColumnGroups.Clear();
    table.Body.Columns.Clear();

    if (addTotalRow)
    {
        TableGroup totalsRowGroup = new TableGroup();
        totalsRowGroup.Name = "TotalsRowGroup";
        table.RowGroups.Add(totalsRowGroup);
        table.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D)));
    }

    for (int i = 0; i < columnNames.Count(); i++)
    {
        string columnName = columnNames[i];
        TableGroup tableGroup = new TableGroup();
        TextBox textBoxHeader = new TextBox();
        TextBox textBoxDetail = new TextBox();

        tableGroup.Name = $"newTableGroup{i}";
        tableGroup.ReportItem = textBoxHeader;
        table.ColumnGroups.Add(tableGroup);

        table.Body.Columns.Add(new TableBodyColumn(columnnWidth));
        table.Body.SetCellContent(0, i, textBoxDetail);

        // 
        // textBoxHeader
        // 
        textBoxHeader.Name = $"textBoxHeader{2 * i}";
        textBoxHeader.Size = new Telerik.Reporting.Drawing.SizeU(columnnWidth, Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D));
        textBoxHeader.StyleName = tableHeaderStyleName;
        textBoxHeader.Value = columnName;
        // 
        // textBoxDetail
        // 
        textBoxDetail.Name = $"textBoxDetail{2 * i}";
        textBoxDetail.Size = new Telerik.Reporting.Drawing.SizeU(columnnWidth, Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D));
        textBoxDetail.StyleName = tableBodyStyleName;
        textBoxDetail.Value = $"= Fields.[{columnName}]";

        if (addTotalRow)
        {
            // 
            // textBoxTotalsRow
            //
            TextBox totalsRowTextBox = new TextBox();
            totalsRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(columnnWidth, Telerik.Reporting.Drawing.Unit.Inch(0.2395833283662796D));
            totalsRowTextBox.StyleName = tableHeaderStyleName;
            totalsRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            totalsRowTextBox.Name = "totalsRowTextBox" + i.ToString();
            totalsRowTextBox.Value = $"= Sum(Fields.[{columnName}])";

            table.Body.SetCellContent(1, i, totalsRowTextBox);
        }
    }
}

DataTable GetData(int columnsCount, int rowsCount)
{
    DataTable dt = new DataTable();
    dt.Clear();

    for (int col = 0; col < columnsCount; col++)
    {
        string columnName = $"Column-{col}";
        dt.Columns.Add(columnName);
        dt.Columns[columnName].DataType = typeof(int);
    }

    for (int row = 0; row < rowsCount; row++)
    {
        DataRow currentRow = dt.NewRow();

        for (int col = 0; col < columnsCount; col++)
        {
            string currentColumnName = dt.Columns[col].ColumnName;
            int currentCellValue = col + row + 1;
            currentRow[currentColumnName] = currentCellValue;
        }

        dt.Rows.Add(currentRow);
    }

    return dt;
}

Report DeserializeReport(string trdxFileName)
{
    System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
    settings.IgnoreWhitespace = true;

    using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(trdxFileName, settings))
    {
        Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer =
            new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

        Report report = (Report)
            xmlSerializer.Deserialize(xmlReader);

        return report;
    }
}

void SerializeReport(Report report, string reportName)
{
    using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(reportName))
    {
        Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer =
            new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

        xmlSerializer.Serialize(xmlWriter, report);
    }
}
