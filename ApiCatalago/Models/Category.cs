using System.Collections.ObjectModel;

namespace ApiCatalago.Models;

public class Category
{
    // Iniciando a coleção de produtos 
    public Category()
    {
        Products = new Collection<Product>();
    }
    
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    
    // Propriedade de navegação para relacionamento 1:n
    public ICollection<Product> Products { get; set; }
}