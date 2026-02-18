using System.ComponentModel.DataAnnotations;
using Homesplash.Models;

namespace Homesplash.Dtos;

public record CategoryDto(int Id, string Name) {
    public static CategoryDto FromModel(Category category) => new(category.Id, category.Name);
}