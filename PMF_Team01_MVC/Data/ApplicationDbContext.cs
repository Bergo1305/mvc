using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMF_Team01_MVC.Models;

namespace PMF_Team01_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Image> Image { get; set; }
        //public DbSet<Drzava> Drzava { get; set; }
        //public DbSet<Grad> Grad { get; set; }
        public DbSet<Rad> Rad { get; set; }
        public DbSet<JavniKomentar> JavniKomentar { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<AutorRad> AutorRad { get; set; }
        public DbSet<Oblast> Oblast { get; set; }
        public DbSet<OblastRecenzent> OblastRecenzent { get; set; }
        public DbSet<OblastRad> OblastRad { get; set; }

        public DbSet<RecenzentRad> RecenzentRad { get; set; }
        public DbSet<PrivatniKomentar> PrivatniKomentar { get; set; }
        public DbSet<KomentarDokument> KomentarDokument { get; set; }

        public DbSet<Mentor> Mentor { get; set; }

        public DbSet<EKnjiga> EKnjiga { get; set; }
        public DbSet<Ideja> Ideja { get; set; }
        public DbSet<StudentskiRad> StudentskiRad { get; set; }
        public DbSet<RecenziraniRad> RecenziraniRad { get; set; }

        public DbSet<Recenzija> Recenzija { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<KategorijaIdeja> KategorijaIdeja { get; set; }
        public DbSet<Publikacija> Publikacija { get; set; }
    }
}
