using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Floodless_MVC.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_FLOODLESS_VOLUNTARIO",
                columns: table => new
                {
                    id_voluntario = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nm_voluntario = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    email_voluntário = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ds_contato = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dt_registro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FLOODLESS_VOLUNTARIO", x => x.id_voluntario);
                });

            migrationBuilder.CreateTable(
                name: "TB_FLOODLESS_RECURSO",
                columns: table => new
                {
                    id_recurso = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nm_recurso = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    tp_recurso = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    qt_recurso = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    dt_criacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    VoluntarioId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FLOODLESS_RECURSO", x => x.id_recurso);
                    table.ForeignKey(
                        name: "FK_TB_FLOODLESS_RECURSO_TB_FLOODLESS_VOLUNTARIO_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "TB_FLOODLESS_VOLUNTARIO",
                        principalColumn: "id_voluntario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_FLOODLESS_RECURSO_VoluntarioId",
                table: "TB_FLOODLESS_RECURSO",
                column: "VoluntarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_FLOODLESS_RECURSO");

            migrationBuilder.DropTable(
                name: "TB_FLOODLESS_VOLUNTARIO");
        }
    }
}
