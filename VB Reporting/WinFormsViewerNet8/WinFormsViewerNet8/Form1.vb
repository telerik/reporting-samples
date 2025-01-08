Imports Telerik.Reporting

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim reportSource1 As UriReportSource = New UriReportSource()
        reportSource1.Uri = "Reports\SampleReport.trdp"
        reportSource1.Parameters.Add("Parameter1_Name", "Parameter1_Value")
        reportSource1.Parameters.Add("Parameter2_Name", "Parameter2_Value")

        Dim reportViewer = New Telerik.ReportViewer.WinForms.ReportViewer()
        reportViewer.ReportSource = reportSource1
        reportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        reportViewer.Name = "reportViewer1"
        reportViewer.TabIndex = 1
        reportViewer.RefreshReport()

        Me.Controls.Add(reportViewer)
    End Sub
End Class
