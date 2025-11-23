using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalCheck.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GS_DICA",
                columns: table => new
                {
                    ID_DICA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITULO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    CATEGORIA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CONDICAO_APLICACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_DICA", x => x.ID_DICA);
                });

            migrationBuilder.CreateTable(
                name: "GS_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    CARGO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    MODALIDADE_TRABALHO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "GS_CHECKIN",
                columns: table => new
                {
                    ID_CHECKIN = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_CHECKIN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NIVEL_ESTRESSE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NIVEL_MOTIVACAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NIVEL_CANSACO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NIVEL_SATISFACAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    QUALIDADE_SONO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LOCAL_TRABALHO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    OBSERVACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_CHECKIN", x => x.ID_CHECKIN);
                    table.ForeignKey(
                        name: "FK_GS_CHECKIN_GS_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "GS_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GS_INSIGHT",
                columns: table => new
                {
                    ID_INSIGHT = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    DATA_IDENTIFICACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_INSIGHT", x => x.ID_INSIGHT);
                    table.ForeignKey(
                        name: "FK_GS_INSIGHT_GS_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "GS_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GS_INSIGHT_DICA",
                columns: table => new
                {
                    ID_INSIGHT_DICA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_INSIGHT = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_DICA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_RECOMENDACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    STATUS_VISUALIZACAO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_INSIGHT_DICA", x => x.ID_INSIGHT_DICA);
                    table.ForeignKey(
                        name: "FK_GS_INSIGHT_DICA_GS_DICA_ID_DICA",
                        column: x => x.ID_DICA,
                        principalTable: "GS_DICA",
                        principalColumn: "ID_DICA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GS_INSIGHT_DICA_GS_INSIGHT_ID_INSIGHT",
                        column: x => x.ID_INSIGHT,
                        principalTable: "GS_INSIGHT",
                        principalColumn: "ID_INSIGHT",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GS_CHECKIN_ID_USUARIO",
                table: "GS_CHECKIN",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_GS_INSIGHT_ID_USUARIO",
                table: "GS_INSIGHT",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_GS_INSIGHT_DICA_ID_DICA",
                table: "GS_INSIGHT_DICA",
                column: "ID_DICA");

            migrationBuilder.CreateIndex(
                name: "IX_GS_INSIGHT_DICA_ID_INSIGHT",
                table: "GS_INSIGHT_DICA",
                column: "ID_INSIGHT");

            migrationBuilder.CreateIndex(
                name: "IX_GS_USUARIO_EMAIL",
                table: "GS_USUARIO",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GS_CHECKIN");

            migrationBuilder.DropTable(
                name: "GS_INSIGHT_DICA");

            migrationBuilder.DropTable(
                name: "GS_DICA");

            migrationBuilder.DropTable(
                name: "GS_INSIGHT");

            migrationBuilder.DropTable(
                name: "GS_USUARIO");
        }
    }
}
