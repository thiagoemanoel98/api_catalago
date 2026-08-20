using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO \"Products\" (\"Name\", \"Description\", \"Price\", \"ImageUrl\", \"Stock\", \"RegistrationDate\", \"CategoryId\" )" +
                   "values ('Coca-Cola 0', 'Coca Cola 0 acuçar bom para dieta, 350ml', 10, 'coca.jpg', 20, now(), 1)");
           
            mb.Sql("INSERT INTO \"Products\" (\"Name\", \"Description\", \"Price\", \"ImageUrl\", \"Stock\", \"RegistrationDate\", \"CategoryId\" )" +
                   "values ('Lanche de Atum', 'Lanche de Atum com Maionese', 9.0, 'atum.jpg', 40, now(), 2)");

            
            mb.Sql("INSERT INTO \"Products\" (\"Name\", \"Description\", \"Price\", \"ImageUrl\", \"Stock\", \"RegistrationDate\", \"CategoryId\" )" +
                   "values ('Pudim', 'pudim 100 g', 6, 'pudim.jpg', 40, now(), 3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
