using Homesplash.Models;

namespace Homesplash.Dtos;

public record TileDetailsDto(int Id, string Name, string Link, int CategoryId) {
    public static TileDetailsDto FromModel(Tile tile) =>
        new(tile.Id, tile.Name, tile.Link, tile.CategoryId);
}
