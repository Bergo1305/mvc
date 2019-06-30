using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.Admin;
using PMF_Team01_MVC.Models.RadViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FITJournalTest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, 
            IHostingEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminRadoviIndex(string title = "", string keywords = "", string type = "", string status = "")
        {
            var currentUsr = _userManager.GetUserId(User);

            var allTitles = _context.Rad.ToList();

            //filter by keywords
            if (!String.IsNullOrEmpty(keywords))
                allTitles = allTitles.Where(r => r.KeyWords.ToLower().Contains(keywords.ToLower())).ToList();

            //ukljuciti autore radova (koristiti autorRad)
            var sviRadovi = allTitles.Select(r => new RadoviIndexVM
            {
                Id = r.Id,
                Naslov = r.Naslov,
                OcjenaAdmina = r.ApprovedByAdmin,
                BrojPozitivnihOcjena = r.BrojPozitivnihOcjena,
                TipRada = r.Tip
            }).ToList();
            
            //filter svih radova
            //u slucaju da admin ima ulogu autora ili recenzenta
            //nece vidjeti svoje radove na ovoj listi jer ne moze uredjivati vlastite
            //radove na taj nacin
            var radovi = new List<RadoviIndexVM>();

            foreach(var item in sviRadovi)
            {
                if (_context.AutorRad.Any(r =>r.RadId == item.Id && r.AuthorId == currentUsr) == false && _context.RecenzentRad.Any(r => r.RadId == item.Id && r.RecenzentId == currentUsr) == false)
                    radovi.Add(item);
            }
            //------------------------------
            
            if (!String.IsNullOrEmpty(title))
                radovi = radovi.Where(r => r.Naslov.ToLower().Contains(title.ToLower())).ToList();

            if(type != "all")
                radovi = radovi.Where(r => r.TipRada.ToLower().Contains(type.ToLower())).ToList();
            
            if(status != "all")
            {
                switch (status)
                {
                    case "not decided":
                        radovi = radovi.Where(r => r.OcjenaAdmina == null).ToList();
                        break;
                    case "approved":
                        radovi = radovi.Where(r => r.OcjenaAdmina == true).ToList();
                        break;
                    case "denied":
                        radovi = radovi.Where(r => r.OcjenaAdmina == false).ToList();
                        break;
                }
            }

            var model = new RadoviIndexListVM
            {
                Radovi = radovi
            };

            return View(model);
        }

        public IActionResult OcijeniRad(int id)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            List<SelectListItem> ocjene = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Not Decided",
                    Value = "-1"
                },
                new SelectListItem
                {
                    Text = "Accepted",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Denied",
                    Value = "0"
                }
            };

            var model = new OcijeniRadVM
            {
                Naslov = rad.Naslov,
                RadId = rad.Id,
                Ocjene = ocjene
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult OcijeniRad(OcijeniRadVM model)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == model.RadId);

            _context.Update(rad);

            switch (model.Ocjena)
            {
                case "-1":
                    rad.ApprovedByAdmin = null;
                    rad.DatumObjavljivanja = null;
                    break;
                case "0":
                    rad.ApprovedByAdmin = false;
                    rad.DatumObjavljivanja = null;
                    break;

                case "1":
                    rad.ApprovedByAdmin = true;
                    rad.DatumObjavljivanja = DateTime.Now;
                    break;
            }

            _context.SaveChanges();

            return RedirectToAction("AdminRadDetails", "Admin", new {@id= model.RadId});
        }

        public IActionResult AdminRadDetails(int id)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            var model = new AdminRadDetails
            {
                RadId = rad.Id,
                Naziv = rad.Naslov,
                ApprovedByAdmin = rad.ApprovedByAdmin,
                BrojPozitivnihOcjena = rad.BrojPozitivnihOcjena,
                Apstrakt = rad.Apstrakt,
                TipRada = rad.Tip,
                UploadDate = rad.UploadDate,
                PublishDate = rad.DatumObjavljivanja,
                OstaliAutori = rad.OstaliAutori
            };


            if(rad.Tip != "Ideja")
            {
                var oblasti = _context.OblastRad.Include(o => o.Oblast).Where(o => o.RadId == rad.Id).ToList();

                List<string> oblastiRada = new List<string>();

                foreach(var item in oblasti)
                {
                    oblastiRada.Add(item.Oblast.Naziv);
                }

                model.Oblasti = oblastiRada;
            }


            if (rad.Tip == "Recenzirani")
                model.TipRecenziranogRada = _context.RecenziraniRad.FirstOrDefault(r => r.RecenziraniRadId == rad.Id).TipRecenziranogRada;
            else if (rad.Tip == "Studentski")
            {
                var studentski = _context.StudentskiRad
                    .Include(s => s.Mentor)
                    .FirstOrDefault(s => s.StudentskiRadId == rad.Id);

                model.TipStudentskogRada = studentski.TipStudentskogRada;
                model.Mentor = studentski.Mentor.Ime + " " + studentski.Mentor.Prezime;
            }
            else if (rad.Tip == "Ideja") { 
                model.TekstIdeje = _context.Ideja.FirstOrDefault(i => i.IdejaId == rad.Id).TekstIdeje;

                var kategorije = _context.KategorijaIdeja.Include(k => k.Kategorija).Where(k => k.IdejaId == rad.Id).ToList();

                List<string> kategorijeIdeje = new List<string>();

                foreach(var item in kategorije)
                {
                    kategorijeIdeje.Add(item.Kategorija.Naziv);
                }

                model.Kategorije = kategorijeIdeje;
            }

            if (rad.Tip == "EKnjiga")
            {

                model.recenzije = new List<string>();

                var recenzentRad = _context.RecenzentRad
                    .Include(r => r.Recenzent)
                    .Include(r => r.Rad)
                    .Where(r => r.RadId == rad.Id)
                    .ToList();

                string naslovKnjige = rad.Naslov;


                foreach (var item in recenzentRad)
                {
                    string folderPath = Path.Combine(_environment.WebRootPath, "Uploads",
                       item.Recenzent.UserName, "Recenzije", naslovKnjige);


                    System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                    //directory exists only if reviewer added the review file
                    if (di.Exists)
                    {
                        //get all files from subfolder
                        var fileList = System.IO.Directory.GetFiles(folderPath);

                        //check if directory contains any files
                        if (fileList.Length > 0)
                        {
                            //get the name of first file
                            var file = Path.GetFileName(fileList[0]);
                            //full folder path
                            folderPath = "/Uploads/" + item.Recenzent.UserName + "/Recenzije/" + naslovKnjige;
                            //full file path
                            var filePath = folderPath + "/" + file;

                            model.recenzije.Add(filePath);
                        }
                    }
                }
            }


            var autorRad = _context.AutorRad
                .Include(a=>a.Author)
                .FirstOrDefault(a => a.RadId == model.RadId);

            model.GlavniAutor = autorRad.Author.Title + " " + autorRad.Author.FirstName + " " + autorRad.Author.LastName;

            model.DocumentExists = _context.Document.Any(d => d.RadId == model.RadId);

            return View(model);
        }

        public IActionResult AdminPrivateComments(int RadId)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == RadId);

            var privateComments = _context.PrivatniKomentar
                .Include(r=>r.AutorKomentara)
                .Where(r => r.RadId == RadId)
                .Select(r => new AdminPrivateCommentsVM
                {
                    AutorId = r.AutorKomentaraId,
                    Autor = r.AutorKomentara.UserName,
                    Sadrzaj = r.Sadrzaj,
                    KomentarId = r.Id,
                    FilePath = "/Uploads/KomentarDocs/" + r.Id + "/" + _context.KomentarDokument.FirstOrDefault(d => d.KomentarDokumentId == r.Id).FileName
                });

            var model = new AdminPrivateCommentsListVM
            {
                Komentari = privateComments,
                RadId = RadId
            };

            return View(model);
        }

        //Prikaz svih ocjena od strane recenzenata
        public IActionResult ReviewerCount(int radId)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == radId);

            var radReview = _context.RecenzentRad
                .Include(r=>r.Recenzent)
                .Where(r => r.RadId == radId);

            var model = new ReviewerCountVM
            {
                RadId = rad.Id,
                Naziv = rad.Naslov,
                RecenzentOcjena = radReview.Select(rr => new RecenzentOcjena
                {
                    Id = rr.RecenzentId,
                    Username = rr.Recenzent.UserName,
                    Ocjena = rr.ApprovedByRecenzent
                }).ToList()
            };
            
            return View(model);
        }
    }
}