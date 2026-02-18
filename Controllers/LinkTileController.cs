using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Homesplash.Data;
using Homesplash.Dtos;
using Homesplash.Models;
using Homesplash.Logos;

namespace Homesplash.Controllers;

[ApiController]
[Route("linktiles")]
public class LinkTileController(LinkTileContext dbContext, ILogoQueue logoQueue) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TileSummaryDto>>> GetAll()
    {
        var tiles = await dbContext.Tiles
            .Include(t => t.Category)
            .AsNoTracking()
            .Select(t => new TileSummaryDto(t.Id, t.Name, t.Link, t.Category!.Name, t.CategoryId))
            .ToListAsync();
        return Ok(tiles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tile = await dbContext.Tiles.FindAsync(id);
        return tile == null ? NotFound() : Ok(TileDetailsDto.FromModel(tile));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLinkTileDto newTile)
    {
        var uri = await LogoUrlResolver.GetResolvedUri(newTile.Link);
        var tile = new Tile
        {
            Name = newTile.Name,
            Link = uri?.AbsoluteUri ?? newTile.Link,
            CategoryId = newTile.CategoryId
        };
        dbContext.Tiles.Add(tile);
        await dbContext.SaveChangesAsync();
        logoQueue.Enqueue(tile.Link);
        return CreatedAtAction(nameof(GetById), new { id = tile.Id }, TileDetailsDto.FromModel(tile));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTile(int id, [FromBody] CreateLinkTileDto updatedTile)
    {
        var tile = await dbContext.Tiles.FindAsync(id);
        if (tile == null) return NotFound();

        tile.Name = updatedTile.Name;
        tile.Link = updatedTile.Link;
        tile.CategoryId = updatedTile.CategoryId;
        dbContext.Entry(tile).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var tile = await dbContext.Tiles.FindAsync(id);
        if (tile == null) return NotFound();

        dbContext.Tiles.Remove(tile);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}