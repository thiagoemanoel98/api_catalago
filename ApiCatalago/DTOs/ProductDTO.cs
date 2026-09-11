using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalago.DTOs;

public class ProductDTO
{ 
    public int ProductId { get; set; }
  
    [Range(1, int.MaxValue, ErrorMessage = "Categoria é obrigatória")]
    public int CategoryId { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(300)] 
    public string? Description { get; set; }
    
    [Required]
    public decimal? Price { get; set; }
    
    [Required]
    [StringLength(300)] 
    public string? ImageUrl { get; set; }

}