namespace Homesplash.Models;

public class Tile {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Link { get; set; }
    public string? IconLink { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}