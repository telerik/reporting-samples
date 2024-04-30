namespace MauiBlazorViewer
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content) =>
            await System.Text.Json.JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync());
    }
}
