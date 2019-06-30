using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PMF_Team01_MVC.Migrations
{
    public partial class addedpublikacija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublikacijaId",
                table: "Rad",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Publikacija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrojPublikacije = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publikacija", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rad_PublikacijaId",
                table: "Rad",
                column: "PublikacijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rad_Publikacija_PublikacijaId",
                table: "Rad",
                column: "PublikacijaId",
                principalTable: "Publikacija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rad_Publikacija_PublikacijaId",
                table: "Rad");

            migrationBuilder.DropTable(
                name: "Publikacija");

            migrationBuilder.DropIndex(
                name: "IX_Rad_PublikacijaId",
                table: "Rad");

            migrationBuilder.DropColumn(
                name: "PublikacijaId",
                table: "Rad");
        }
    }
}
