Partial Class Report1

    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim DataColumn1 As Telerik.Reporting.DataColumn = New Telerik.Reporting.DataColumn()
        Dim TableGroup1 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim TableGroup2 As Telerik.Reporting.TableGroup = New Telerik.Reporting.TableGroup()
        Dim ReportParameter1 As Telerik.Reporting.ReportParameter = New Telerik.Reporting.ReportParameter()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.csvDataSource1 = New Telerik.Reporting.CsvDataSource()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.list1 = New Telerik.Reporting.List()
        Me.panel1 = New Telerik.Reporting.Panel()
        Me.textBox1 = New Telerik.Reporting.TextBox()
        Me.objectDataSource1 = New Telerik.Reporting.ObjectDataSource()
        Me.pageFooterSection1 = New Telerik.Reporting.PageFooterSection()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'csvDataSource1
        '
        DataColumn1.Name = "Text"
        Me.csvDataSource1.Columns.Add(DataColumn1)
        Me.csvDataSource1.FieldSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(44)}
        Me.csvDataSource1.HasHeaders = True
        Me.csvDataSource1.Name = "csvDataSource1"
        Me.csvDataSource1.RecordSeparators = New Char() {Global.Microsoft.VisualBasic.ChrW(13), Global.Microsoft.VisualBasic.ChrW(10)}
        Me.csvDataSource1.Source = "Text" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "0" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "select me to display a very long text"
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.3R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.list1})
        Me.detail.Name = "detail"
        '
        'list1
        '
        Me.list1.Body.Columns.Add(New Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(6.5R)))
        Me.list1.Body.Rows.Add(New Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.3R)))
        Me.list1.Body.SetCellContent(0, 0, Me.panel1)
        TableGroup1.Name = "ColumnGroup"
        Me.list1.ColumnGroups.Add(TableGroup1)
        Me.list1.DataSource = Me.objectDataSource1
        Me.list1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.panel1})
        Me.list1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0R), Telerik.Reporting.Drawing.Unit.Inch(0R))
        Me.list1.Name = "list1"
        TableGroup2.Groupings.Add(New Telerik.Reporting.Grouping(Nothing))
        TableGroup2.Name = "DetailGroup"
        Me.list1.RowGroups.Add(TableGroup2)
        Me.list1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.5R), Telerik.Reporting.Drawing.Unit.Inch(0.3R))
        '
        'panel1
        '
        Me.panel1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.textBox1})
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.5R), Telerik.Reporting.Drawing.Unit.Inch(0.3R))
        '
        'textBox1
        '
        Me.textBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0R), Telerik.Reporting.Drawing.Unit.Inch(0R))
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.5R), Telerik.Reporting.Drawing.Unit.Inch(0.3R))
        Me.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        Me.textBox1.Value = "= Fields.Item"
        '
        'objectDataSource1
        '
        Me.objectDataSource1.DataMember = "LongText"
        Me.objectDataSource1.DataSource = GetType(ClassLibrary1.MyHelper)
        Me.objectDataSource1.Name = "objectDataSource1"
        Me.objectDataSource1.Parameters.Add(New Telerik.Reporting.ObjectDataSourceParameter("originalText", GetType(String), "= Parameters.originalText.Value"))
        '
        'pageFooterSection1
        '
        Me.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.0R)
        Me.pageFooterSection1.Name = "pageFooterSection1"
        '
        'Report1
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.detail, Me.pageFooterSection1})
        Me.Name = "Report1"
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R), Telerik.Reporting.Drawing.Unit.Inch(1.0R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter
        ReportParameter1.AutoRefresh = True
        ReportParameter1.AvailableValues.DataSource = Me.csvDataSource1
        ReportParameter1.AvailableValues.DisplayMember = "= Fields.Text"
        ReportParameter1.AvailableValues.ValueMember = "= Fields.Text"
        ReportParameter1.Name = "originalText"
        ReportParameter1.Value = "= Fields.Text"
        ReportParameter1.Visible = True
        Me.ReportParameters.Add(ReportParameter1)
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.TextItemBase)), New Telerik.Reporting.Drawing.TypeSelector(GetType(Telerik.Reporting.HtmlTextBox))})
        StyleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        StyleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2.0R)
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1})
        Me.Width = Telerik.Reporting.Drawing.Unit.Inch(6.5R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents detail As Telerik.Reporting.DetailSection
    Friend WithEvents pageFooterSection1 As Telerik.Reporting.PageFooterSection
    Private WithEvents csvDataSource1 As Telerik.Reporting.CsvDataSource
    Private WithEvents objectDataSource1 As Telerik.Reporting.ObjectDataSource
    Private WithEvents list1 As Telerik.Reporting.List
    Private WithEvents panel1 As Telerik.Reporting.Panel
    Private WithEvents textBox1 As Telerik.Reporting.TextBox
End Class