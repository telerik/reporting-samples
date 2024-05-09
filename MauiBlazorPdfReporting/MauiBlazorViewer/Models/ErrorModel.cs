namespace MauiBlazorViewer.Models
{
    public class ErrorModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("error")]
        public string Error { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("error_description")]
        public string Description { get; set; }
    }
}
