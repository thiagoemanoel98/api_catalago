using ApiCatalago.Models;

namespace ApiCatalago.DTOs;

public class ProductDTOUpdateResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public int CategoryId { get; set; }
}