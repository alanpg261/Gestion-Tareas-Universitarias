using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionTareasUniversitarias.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioAndEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completada",
                table: "Tareas");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Materias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_UsuarioId",
                table: "Materias",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materias_AspNetUsers_UsuarioId",
                table: "Materias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materias_AspNetUsers_UsuarioId",
                table: "Materias");

            migrationBuilder.DropIndex(
                name: "IX_Materias_UsuarioId",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Materias");

            migrationBuilder.AddColumn<bool>(
                name: "Completada",
                table: "Tareas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
