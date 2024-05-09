namespace MauiBlazorViewer.Models
{
    public class DocumentInfoModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("documentReady")]
        public bool DocumentReady { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("PageCount")]
        public int PageCount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("DocumentMapAvailable")]
        public bool DocumentMapAvailable { get; set; }
    }
}
