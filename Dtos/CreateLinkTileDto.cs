using System.ComponentModel.DataAnnotations;
using Homesplash.Models;

namespace Homesplash.Dtos;

public record CreateLinkTileDto([Required] string Name, [Required] string Link, int CategoryId) {
    public static Tile ToModel(int id, CreateLinkTileDto tile) => new() {
        Id = id,
        Name = tile.Name,
        Link = tile.Link,
        CategoryId = tile.CategoryId
    };
}