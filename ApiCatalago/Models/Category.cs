using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.Models;

public class Category
{
    // Iniciando a coleção de produtos 
    public Category()
    {
        Products = new Collection<Product>();
    }
    
    [Key]
    public int CategoryId { get; set; }
    
    [Required]
    [StringLength(80)] // 80 bytes
    public string? Name { get; set; }
    
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    
    // Propriedade de navegação para relacionamento 1:n
    public ICollection<Product>? Products { get; set; }
}