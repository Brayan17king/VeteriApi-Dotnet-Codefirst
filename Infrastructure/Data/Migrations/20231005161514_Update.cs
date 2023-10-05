using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cliente_ciudad_CiudadId",
                table: "cliente");

            migrationBuilder.DropIndex(
                name: "IX_cliente_CiudadId",
                table: "cliente");

            migrationBuilder.DropColumn(
                name: "CiudadId",
                table: "cliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CiudadId",
                table: "cliente",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cliente_CiudadId",
                table: "cliente",
                column: "CiudadId");

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_ciudad_CiudadId",
                table: "cliente",
                column: "CiudadId",
                principalTable: "ciudad",
                principalColumn: "Id");
        }
    }
}
