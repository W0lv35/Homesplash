namespace Homesplash.Logos;

internal static class LogoUrlResolver {
    public static async Task<Uri?> GetResolvedUri(string rawUrl) {
        var uriString = PrepareUrl(rawUrl);
        try {
            var uri = new Uri(uriString);
            if (await ValidateUrl(uri)) return uri;

            var builder = new UriBuilder(uri) { Scheme = uri.Scheme == "https" ? "http" : "https" };
            if (await ValidateUrl(builder.Uri)) return builder.Uri;

            return null;
        } catch { return null; }
    }

    private static string PrepareUrl(string rawUrl) {
        if (rawUrl.Contains("://")) return rawUrl;
        return "https://" + rawUrl;
    }

    private static async Task<bool> ValidateUrl(Uri url) {
        using var client = new HttpClient();
        try {
            var response = await client.GetAsync(url);
            return response.IsSuccessStatusCode;
        } catch { return false; }
    }
}