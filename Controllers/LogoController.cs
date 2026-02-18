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

            if (!System.IO.File.Exists(filePath)) {
                return NotFound("Logo not found. It may be in the queue for processing.");
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        } catch {
            return BadRequest("Invalid URL format");
        }
    }
}