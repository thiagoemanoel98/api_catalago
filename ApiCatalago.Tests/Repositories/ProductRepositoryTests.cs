using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using ApiCatalago.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiCatalago.Tests.Repositories;

public class ProductRepositoryTests
{
    [Fact]
    public async Task GetProductsAsync_returns_the_requested_page_with_metadata()
    {
        await using var context = CreateContext();
        await context.Products!.AddRangeAsync(
            CreateProduct(1, 10m),
            CreateProduct(2, 20m),
            CreateProduct(3, 30m),
            CreateProduct(4, 40m),
            CreateProduct(5, 50m));
        await context.SaveChangesAsync();

        var repository = new ProductRepository(context);

        var page = await repository.GetProductsAsync(new ProductsParameters
        {
            PageNumber = 2,
            PageSize = 2
        });

        Assert.Equal(new[] { 3, 4 }, page.Select(product => product.ProductId));
        Assert.Equal(5, page.TotalCount);
        Assert.Equal(2, page.CurrentPage);
        Assert.Equal(3, page.TotalPages);
        Assert.True(page.HasNext);
        Assert.True(page.HasPrevius);
    }

    [Fact]
    public async Task GetProductsPriceFilterAsync_filters_orders_and_pages_in_the_database_query()
    {
        await using var context = CreateContext();
        await context.Products!.AddRangeAsync(
            CreateProduct(1, 10m),
            CreateProduct(2, 50m),
            CreateProduct(3, 30m),
            CreateProduct(4, 40m),
            CreateProduct(5, 20m));
        await context.SaveChangesAsync();

        var repository = new ProductRepository(context);

        var page = await repository.GetProductsPriceFilterAsync(new ProductsFilterPrice
        {
            Price = 20m,
            PriceCriterion = "maior",
            PageNumber = 2,
            PageSize = 2
        });

        Assert.Equal(new[] { 2 }, page.Select(product => product.ProductId));
        Assert.Equal(3, page.TotalCount);
        Assert.Equal(2, page.CurrentPage);
        Assert.Equal(2, page.TotalPages);
        Assert.False(page.HasNext);
        Assert.True(page.HasPrevius);
    }

    [Fact]
    public async Task GetCategoriesNameFilterAsync_returns_the_requested_page_with_metadata()
    {
        await using var context = CreateContext();
        await context.Categories!.AddRangeAsync(
            new Category { CategoryId = 1, Name = "Alimentos", ImageUrl = "alimentos.png" },
            new Category { CategoryId = 2, Name = "Eletrônicos", ImageUrl = "eletronicos.png" },
            new Category { CategoryId = 3, Name = "Eletrônicos premium", ImageUrl = "premium.png" });
        await context.SaveChangesAsync();

        var repository = new CategoryRepository(context);

        var page = await repository.GetCategoriesNameFilterAsync(new CategoriesFilterName
        {
            Name = "Eletrônicos",
            PageNumber = 1,
            PageSize = 1
        });

        var category = Assert.Single(page);
        Assert.Equal(2, category.CategoryId);
        Assert.Equal(2, page.TotalCount);
        Assert.Equal(1, page.CurrentPage);
        Assert.Equal(2, page.TotalPages);
        Assert.True(page.HasNext);
        Assert.False(page.HasPrevius);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static Product CreateProduct(int id, decimal price) => new()
    {
        ProductId = id,
        Name = $"Produto {id}",
        Description = $"Descrição {id}",
        Price = price,
        ImageUrl = $"produto-{id}.png",
        CategoryId = 1
    };
}
