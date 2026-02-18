using Homesplash.Models;
using Microsoft.EntityFrameworkCore;

namespace Homesplash.Data;

public class LinkTileContext(DbContextOptions<LinkTileContext> options) : DbContext(options) {
    public DbSet<Tile> Tiles => Set<Tile>();
    public DbSet<Category> Categories => Set<Category>();
}