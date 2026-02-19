using Microsoft.AspNetCore.Mvc;

namespace Homesplash.Controllers;

[ApiController]
[Route("logo")]
public class LogoController : ControllerBase {
    [HttpGet]
    public IActionResult GetLogo([FromQuery] string url) {
        if (string.IsNullOrWhiteSpace(url)) return BadRequest("URL is required");

        try {
            var uri = new Uri(url.StartsWith("http") ? url : $"https://{url}");
            var host = uri.Host;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logos", "Images", $"{host}.jpg");
            var defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "Logos", "Images", "default.png");
            if (!System.IO.File.Exists(filePath)) {
                return System.IO.File.Exists(defaultPath) ? File(System.IO.File.OpenRead(defaultPath), "image/png" ) : NotFound();
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        } catch {
            return BadRequest("Invalid URL format");
        }
    }
}