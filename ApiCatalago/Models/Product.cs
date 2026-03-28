using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalago.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(300)] 
    public string? Description { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")] // precisao de 10 digitos com 2 casas decimais
    public decimal? Price { get; set; }
    
    [Required]
    [StringLength(300)] 
    public string? ImageUrl { get; set; }
    
    public int Stock { get; set; }
    
    public DateTime? RegistrationDate { get; set; }
    
    public int CategoryId { get; set; }
    
    // Não exibe no Json
    [JsonIgnore]
    public Category? Category { get; set; }
}