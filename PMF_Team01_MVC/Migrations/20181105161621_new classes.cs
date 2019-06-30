using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PMF_Team01_MVC.Migrations
{
    public partial class newclasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyWords",
                table: "Rad",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EKnjiga",
                columns: table => new
                {
                    EKnjigaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EKnjiga", x => x.EKnjigaId);
                    table.ForeignKey(
                        name: "FK_EKnjiga_Rad_EKnjigaId",
                        column: x => x.EKnjigaId,
                        principalTable: "Rad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ideja",
                columns: table => new
                {
                    IdejaId = table.Column<int>(type: "int", nullable: false),
                    TekstIdeje = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideja", x => x.IdejaId);
                    table.ForeignKey(
                        name: "FK_Ideja_Rad_IdejaId",
                        column: x => x.IdejaId,
                        principalTable: "Rad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mentor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titula = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecenziraniRad",
                columns: table => new
                {
                    RecenziraniRadId = table.Column<int>(type: "int", nullable: false),
                    TipRecenziranogRada = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecenziraniRad", x => x.RecenziraniRadId);
                    table.ForeignKey(
                        name: "FK_RecenziraniRad_Rad_RecenziraniRadId",
                        column: x => x.RecenziraniRadId,
                        principalTable: "Rad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EKnjigaId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recenzija_EKnjiga_EKnjigaId",
                        column: x => x.EKnjigaId,
                        principalTable: "EKnjiga",
                        principalColumn: "EKnjigaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recenzija_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KategorijaIdeja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdejaId = table.Column<int>(type: "int", nullable: false),
                    KategorijaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorijaIdeja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KategorijaIdeja_Ideja_IdejaId",
                        column: x => x.IdejaId,
                        principalTable: "Ideja",
                        principalColumn: "IdejaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KategorijaIdeja_Kategorija_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentskiRad",
                columns: table => new
                {
                    StudentskiRadId = table.Column<int>(type: "int", nullable: false),
                    MentorId = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipStudentskogRada = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentskiRad", x => x.StudentskiRadId);
                    table.ForeignKey(
                        name: "FK_StudentskiRad_Mentor_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentskiRad_Rad_StudentskiRadId",
                        column: x => x.StudentskiRadId,
                        principalTable: "Rad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KategorijaIdeja_IdejaId",
                table: "KategorijaIdeja",
                column: "IdejaId");

            migrationBuilder.CreateIndex(
                name: "IX_KategorijaIdeja_KategorijaId",
                table: "KategorijaIdeja",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzija_EKnjigaId",
                table: "Recenzija",
                column: "EKnjigaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzija_ReviewerId",
                table: "Recenzija",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentskiRad_MentorId",
                table: "StudentskiRad",
                column: "MentorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KategorijaIdeja");

            migrationBuilder.DropTable(
                name: "Recenzija");

            migrationBuilder.DropTable(
                name: "RecenziraniRad");

            migrationBuilder.DropTable(
                name: "StudentskiRad");

            migrationBuilder.DropTable(
                name: "Ideja");

            migrationBuilder.DropTable(
                name: "Kategorija");

            migrationBuilder.DropTable(
                name: "EKnjiga");

            migrationBuilder.DropTable(
                name: "Mentor");

            migrationBuilder.DropColumn(
                name: "KeyWords",
                table: "Rad");
        }
    }
}
