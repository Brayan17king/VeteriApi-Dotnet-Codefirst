using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatetwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cita_Servicio_ServicioId",
                table: "cita");

            migrationBuilder.RenameColumn(
                name: "ServicioId",
                table: "cita",
                newName: "IdServicioFk");

            migrationBuilder.RenameIndex(
                name: "IX_cita_ServicioId",
                table: "cita",
                newName: "IX_cita_IdServicioFk");

            migrationBuilder.AddForeignKey(
                name: "FK_cita_Servicio_IdServicioFk",
                table: "cita",
                column: "IdServicioFk",
                principalTable: "Servicio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cita_Servicio_IdServicioFk",
                table: "cita");

            migrationBuilder.RenameColumn(
                name: "IdServicioFk",
                table: "cita",
                newName: "ServicioId");

            migrationBuilder.RenameIndex(
                name: "IX_cita_IdServicioFk",
                table: "cita",
                newName: "IX_cita_ServicioId");

            migrationBuilder.AddForeignKey(
                name: "FK_cita_Servicio_ServicioId",
                table: "cita",
                column: "ServicioId",
                principalTable: "Servicio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
