namespace Homesplash.Logos;

public interface ILogoService {
    Task FetchAndSaveLogo(string websiteUrl);
}
public class LogoService(IConfiguration configuration) : ILogoService {
    private readonly string _logoToken = configuration["LogoSettings:Token"] 
        ?? throw new InvalidOperationException("Logo Token not found in configuration.");

    public async Task FetchAndSaveLogo(string url) {
        var validUrl = await LogoUrlResolver.GetResolvedUri(url);
        if (validUrl == null) return;

        using var client = new HttpClient();
        byte[] imageBytes;
        try {
            imageBytes = await client.GetByteArrayAsync($"https://img.logo.dev/{validUrl.DnsSafeHost}?theme=dark&format=png&token={_logoToken}");
        } catch { return; }

        var path = Path.Combine("./Logos/Images/", $"{validUrl.DnsSafeHost}.jpg");
        if (File.Exists(path)) return;

        await File.WriteAllBytesAsync(path, imageBytes);
    }
}