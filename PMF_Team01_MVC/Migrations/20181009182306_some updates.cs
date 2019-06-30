using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PMF_Team01_MVC.Migrations
{
    public partial class someupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivatniKomentar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutorKomentaraId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadId = table.Column<int>(type: "int", nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivatniKomentar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivatniKomentar_AspNetUsers_AutorKomentaraId",
                        column: x => x.AutorKomentaraId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivatniKomentar_Rad_RadId",
                        column: x => x.RadId,
                        principalTable: "Rad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecenzentRad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApprovedByRecenzent = table.Column<bool>(type: "bit", nullable: true),
                    RadId = table.Column<int>(type: "int", nullable: false),
                    RecenzentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecenzentRad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecenzentRad_Rad_RadId",
                        column: x => x.RadId,
                        principalTable: "Rad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecenzentRad_AspNetUsers_RecenzentId",
                        column: x => x.RecenzentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KomentarDokument",
                columns: table => new
                {
                    KomentarDokumentId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KomentarDokument", x => x.KomentarDokumentId);
                    table.ForeignKey(
                        name: "FK_KomentarDokument_PrivatniKomentar_KomentarDokumentId",
                        column: x => x.KomentarDokumentId,
                        principalTable: "PrivatniKomentar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivatniKomentar_AutorKomentaraId",
                table: "PrivatniKomentar",
                column: "AutorKomentaraId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivatniKomentar_RadId",
                table: "PrivatniKomentar",
                column: "RadId");

            migrationBuilder.CreateIndex(
                name: "IX_RecenzentRad_RadId",
                table: "RecenzentRad",
                column: "RadId");

            migrationBuilder.CreateIndex(
                name: "IX_RecenzentRad_RecenzentId",
                table: "RecenzentRad",
                column: "RecenzentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KomentarDokument");

            migrationBuilder.DropTable(
                name: "RecenzentRad");

            migrationBuilder.DropTable(
                name: "PrivatniKomentar");
        }
    }
}
