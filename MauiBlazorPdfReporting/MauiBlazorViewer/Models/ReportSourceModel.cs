namespace MauiBlazorViewer.Models
{
    public class ReportSourceModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("Report")]
        public string Report { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("ParameterValues")]
        public IDictionary<string, object> ParameterValues { get; set; }
    }
}
