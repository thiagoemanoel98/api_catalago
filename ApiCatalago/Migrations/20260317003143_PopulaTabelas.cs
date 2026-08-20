using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO \"Categories\" (\"Name\", \"ImageUrl\") values ('Bebidas', 'bebidas.jpg')");
            mb.Sql("INSERT INTO \"Categories\" (\"Name\", \"ImageUrl\") values ('Comidas', 'comida.jpg')");
            mb.Sql("INSERT INTO \"Categories\" (\"Name\", \"ImageUrl\") values ('Sobremesas', 'sobremesa.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
