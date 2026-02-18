using Homesplash.Data;
using Homesplash.Dtos;
using Homesplash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Homesplash.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController(LinkTileContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .Select(cat => CategoryDto.FromModel(cat))
            .ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var category = await dbContext.Categories.FindAsync(id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpGet("tiles/{id}")]
    public async Task<ActionResult<IEnumerable<TileDetailsDto>>> GetTilesByCategoryId(int id)
    {
        var tiles = await dbContext.Tiles
            .Where(tile => tile.CategoryId == id)
            .Select(tile => TileDetailsDto.FromModel(tile))
            .ToListAsync();
        return Ok(tiles);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CreateCategoryDto newCategory)
    {
        var category = new Category { Name = newCategory.Name };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, CategoryDto.FromModel(category));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategory)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category == null) return NotFound();

        category.Name = updateCategory.Name;
        dbContext.Entry(category).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category == null) return NotFound();

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}