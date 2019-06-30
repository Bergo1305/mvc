﻿// <auto-generated />
using PMF_Team01_MVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace PMF_Team01_MVC.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190326193303_added publikacija")]
    partial class addedpublikacija
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FITJournalTest.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Affiliation");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<int>("GradId");

                    b.Property<string>("Initials");

                    b.Property<bool?>("IsEnabled");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Middlename");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("StudentStudyProgram");

                    b.Property<string>("Title");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("GradId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FITJournalTest.Models.AutorRad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<int>("RadId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RadId");

                    b.ToTable("AutorRad");
                });

            modelBuilder.Entity("FITJournalTest.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<int>("RadId");

                    b.Property<bool>("TrenutnaVerzija");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.HasIndex("RadId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("FITJournalTest.Models.Drzava", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv");

                    b.HasKey("Id");

                    b.ToTable("Drzava");
                });

            modelBuilder.Entity("FITJournalTest.Models.EKnjiga", b =>
                {
                    b.Property<int>("EKnjigaId");

                    b.HasKey("EKnjigaId");

                    b.ToTable("EKnjiga");
                });

            modelBuilder.Entity("FITJournalTest.Models.Grad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DrzavaId");

                    b.Property<string>("Naziv");

                    b.HasKey("Id");

                    b.HasIndex("DrzavaId");

                    b.ToTable("Grad");
                });

            modelBuilder.Entity("FITJournalTest.Models.Ideja", b =>
                {
                    b.Property<int>("IdejaId");

                    b.Property<string>("TekstIdeje");

                    b.HasKey("IdejaId");

                    b.ToTable("Ideja");
                });

            modelBuilder.Entity("FITJournalTest.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("FITJournalTest.Models.JavniKomentar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("Datum");

                    b.Property<int>("RadId");

                    b.Property<string>("Sadrzaj");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("RadId");

                    b.ToTable("JavniKomentar");
                });

            modelBuilder.Entity("FITJournalTest.Models.Kategorija", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv");

                    b.Property<string>("Opis");

                    b.HasKey("Id");

                    b.ToTable("Kategorija");
                });

            modelBuilder.Entity("FITJournalTest.Models.KategorijaIdeja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IdejaId");

                    b.Property<int>("KategorijaId");

                    b.HasKey("Id");

                    b.HasIndex("IdejaId");

                    b.HasIndex("KategorijaId");

                    b.ToTable("KategorijaIdeja");
                });

            modelBuilder.Entity("FITJournalTest.Models.KomentarDokument", b =>
                {
                    b.Property<int>("KomentarDokumentId");

                    b.Property<string>("FileName");

                    b.HasKey("KomentarDokumentId");

                    b.ToTable("KomentarDokument");
                });

            modelBuilder.Entity("FITJournalTest.Models.Mentor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ime");

                    b.Property<string>("Prezime");

                    b.Property<string>("Titula");

                    b.HasKey("Id");

                    b.ToTable("Mentor");
                });

            modelBuilder.Entity("FITJournalTest.Models.Oblast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naziv");

                    b.Property<string>("Opis");

                    b.HasKey("Id");

                    b.ToTable("Oblast");
                });

            modelBuilder.Entity("FITJournalTest.Models.OblastRad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OblastId");

                    b.Property<int>("RadId");

                    b.HasKey("Id");

                    b.HasIndex("OblastId");

                    b.HasIndex("RadId");

                    b.ToTable("OblastRad");
                });

            modelBuilder.Entity("FITJournalTest.Models.OblastRecenzent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("OblastId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("OblastId");

                    b.ToTable("OblastRecenzent");
                });

            modelBuilder.Entity("FITJournalTest.Models.PrivatniKomentar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AutorKomentaraId");

                    b.Property<int>("RadId");

                    b.Property<string>("Sadrzaj");

                    b.HasKey("Id");

                    b.HasIndex("AutorKomentaraId");

                    b.HasIndex("RadId");

                    b.ToTable("PrivatniKomentar");
                });

            modelBuilder.Entity("FITJournalTest.Models.Publikacija", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrojPublikacije");

                    b.Property<DateTime?>("DatumIzdavanja");

                    b.Property<string>("Napomena");

                    b.Property<string>("Naziv");

                    b.HasKey("Id");

                    b.ToTable("Publikacija");
                });

            modelBuilder.Entity("FITJournalTest.Models.Rad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("ApprovedByAdmin");

                    b.Property<string>("Apstrakt");

                    b.Property<int>("BrojPozitivnihOcjena");

                    b.Property<DateTime?>("DatumObjavljivanja");

                    b.Property<string>("KeyWords");

                    b.Property<string>("Naslov");

                    b.Property<int?>("PublikacijaId");

                    b.Property<string>("Tip");

                    b.Property<DateTime?>("UploadDate");

                    b.HasKey("Id");

                    b.HasIndex("PublikacijaId");

                    b.ToTable("Rad");
                });

            modelBuilder.Entity("FITJournalTest.Models.RecenzentRad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("ApprovedByRecenzent");

                    b.Property<int>("RadId");

                    b.Property<string>("RecenzentId");

                    b.HasKey("Id");

                    b.HasIndex("RadId");

                    b.HasIndex("RecenzentId");

                    b.ToTable("RecenzentRad");
                });

            modelBuilder.Entity("FITJournalTest.Models.Recenzija", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EKnjigaId");

                    b.Property<string>("FileName");

                    b.Property<string>("ReviewerId");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.HasIndex("EKnjigaId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Recenzija");
                });

            modelBuilder.Entity("FITJournalTest.Models.RecenziraniRad", b =>
                {
                    b.Property<int>("RecenziraniRadId");

                    b.Property<string>("TipRecenziranogRada");

                    b.HasKey("RecenziraniRadId");

                    b.ToTable("RecenziraniRad");
                });

            modelBuilder.Entity("FITJournalTest.Models.StudentskiRad", b =>
                {
                    b.Property<int>("StudentskiRadId");

                    b.Property<int>("MentorId");

                    b.Property<string>("Napomena");

                    b.Property<string>("TipStudentskogRada");

                    b.HasKey("StudentskiRadId");

                    b.HasIndex("MentorId");

                    b.ToTable("StudentskiRad");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FITJournalTest.Models.ApplicationUser", b =>
                {
                    b.HasOne("FITJournalTest.Models.Grad", "Grad")
                        .WithMany()
                        .HasForeignKey("GradId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.AutorRad", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.Document", b =>
                {
                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.EKnjiga", b =>
                {
                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("EKnjigaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.Grad", b =>
                {
                    b.HasOne("FITJournalTest.Models.Drzava", "Drzava")
                        .WithMany()
                        .HasForeignKey("DrzavaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.Ideja", b =>
                {
                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("IdejaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.Image", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("FITJournalTest.Models.JavniKomentar", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.KategorijaIdeja", b =>
                {
                    b.HasOne("FITJournalTest.Models.Ideja", "Ideja")
                        .WithMany()
                        .HasForeignKey("IdejaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FITJournalTest.Models.Kategorija", "Kategorija")
                        .WithMany()
                        .HasForeignKey("KategorijaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.KomentarDokument", b =>
                {
                    b.HasOne("FITJournalTest.Models.PrivatniKomentar", "PrivatniKomentar")
                        .WithOne("KomentarDokument")
                        .HasForeignKey("FITJournalTest.Models.KomentarDokument", "KomentarDokumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.OblastRad", b =>
                {
                    b.HasOne("FITJournalTest.Models.Oblast", "Oblast")
                        .WithMany()
                        .HasForeignKey("OblastId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.OblastRecenzent", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("FITJournalTest.Models.Oblast", "Oblast")
                        .WithMany()
                        .HasForeignKey("OblastId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.PrivatniKomentar", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser", "AutorKomentara")
                        .WithMany()
                        .HasForeignKey("AutorKomentaraId");

                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.Rad", b =>
                {
                    b.HasOne("FITJournalTest.Models.Publikacija", "Publikacija")
                        .WithMany()
                        .HasForeignKey("PublikacijaId");
                });

            modelBuilder.Entity("FITJournalTest.Models.RecenzentRad", b =>
                {
                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FITJournalTest.Models.ApplicationUser", "Recenzent")
                        .WithMany()
                        .HasForeignKey("RecenzentId");
                });

            modelBuilder.Entity("FITJournalTest.Models.Recenzija", b =>
                {
                    b.HasOne("FITJournalTest.Models.EKnjiga", "EKnjiga")
                        .WithMany()
                        .HasForeignKey("EKnjigaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FITJournalTest.Models.ApplicationUser", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId");
                });

            modelBuilder.Entity("FITJournalTest.Models.RecenziraniRad", b =>
                {
                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("RecenziraniRadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FITJournalTest.Models.StudentskiRad", b =>
                {
                    b.HasOne("FITJournalTest.Models.Mentor", "Mentor")
                        .WithMany()
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FITJournalTest.Models.Rad", "Rad")
                        .WithMany()
                        .HasForeignKey("StudentskiRadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FITJournalTest.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FITJournalTest.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
