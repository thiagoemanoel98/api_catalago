using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Context;

/*
 * Faz o mapeamento com o Banco de dados
 */

public class AppDbContext : DbContext
{
    // Construtor que passa parametro options e passa para o construtor da classe base
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }
    
    // Mapeia as entidades para as tabelas
    public DbSet<Category>? Categories { get; set; } 
    public DbSet<Product>? Products { get; set; } 
    
}