using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.RadViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PMF_Team01_MVC.Controllers
{
    public class RadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public RadController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Reader")]
        [HttpGet]
        public IActionResult IndexRadova(string naziv = "", string keywords = "", string type = "")
        {
            var svi = _context.Rad.
                Where(r => r.ApprovedByAdmin == true);


            if (!String.IsNullOrEmpty(type) && type != "all")
            {
                svi = svi.Where(r => r.Tip.ToLower().Contains(type.ToLower()));
            }

            if (!String.IsNullOrEmpty(naziv))
            {
                svi = svi.Where(r => r.Naslov.ToLower().Contains(naziv.ToLower()));
            }

            if (!String.IsNullOrEmpty(keywords))
            {
                svi = svi.Where(r => r.KeyWords.ToLower().Contains(keywords.ToLower()));
            }

            var radovi = svi.Select(r => new RadoviIndexVM
            {
                Id = r.Id,
                Naslov = r.Naslov,
                TipRada = r.Tip,
                PublishDate = r.DatumObjavljivanja,
                Autor = _context.AutorRad.Include(ar => ar.Author).FirstOrDefault(ar => ar.RadId == r.Id).Author.UserName

            }).ToList();

            var model = new RadoviIndexListVM
            {
                Radovi = radovi
            };

            return View(model);
        }

        public IActionResult RadoviIndexByTip(string tip)
        {
            var radovi = _context.Rad
                .Where(r => r.Tip == tip && r.ApprovedByAdmin == true)
                .Select(r => new RadoviIndexVM
                {
                    Naslov = r.Naslov,
                    TipRada = r.Tip,
                    Id = r.Id
                }).ToList();

            var model = new RadoviIndexListVM
            {
                Radovi = radovi
            };
            
            return View(model);
        }

        [Authorize(Roles = "Reader")]
        public IActionResult RadDetails(int id)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            var oblastiRada = _context.OblastRad
                .Include(r => r.Oblast)
                .Where(r => r.RadId == id);

            List<string> ListaOblasti = new List<string>();

            foreach (var item in oblastiRada)
            {
                ListaOblasti.Add(item.Oblast.Naziv);
            }

            var model = new RadDetailsVM
            {
                Id = rad.Id,
                Naslov = rad.Naslov,
                Apstrakt = rad.Apstrakt,
                IsOdobren = rad.ApprovedByAdmin,
                Oblasti = ListaOblasti,
                TipRada = rad.Tip,
                DatumObjavljivanja = rad.DatumObjavljivanja,
                KeyWords = rad.KeyWords,
                OstaliAutori = rad.OstaliAutori
            };

            if (rad.Tip == "Recenzirani")
            {
                var recenzirani = _context.RecenziraniRad.FirstOrDefault(r => r.RecenziraniRadId == rad.Id);
                model.TipRecenziranogRada = recenzirani.TipRecenziranogRada;
            }
            else if (rad.Tip == "Studentski")
            {
                var studentski = _context.StudentskiRad
                    .Include(s=>s.Mentor)
                    .FirstOrDefault(s => s.StudentskiRadId == rad.Id);

                model.TipStudentskogRada = studentski.TipStudentskogRada;
                model.Mentor = studentski.Mentor.Titula + " " + studentski.Mentor.Ime + " " + studentski.Mentor.Prezime;
            }
            else if (rad.Tip == "Ideja")
            {
                var ideja = _context.Ideja.FirstOrDefault(i => i.IdejaId == rad.Id);
                model.TekstIdeje = ideja.TekstIdeje;

                var kategorije = _context.KategorijaIdeja.Include(k => k.Kategorija)
                    .Where(k => k.IdejaId == rad.Id);

                model.Kategorije = new List<string>();

                foreach(var item in kategorije)
                {
                    model.Kategorije.Add(item.Kategorija.Naziv);
                }

            }
            else if (rad.Tip == "EKnjiga")
            {

                model.recenzije = new List<string>();

                var recenzentRad = _context.RecenzentRad
                    .Include(r=>r.Recenzent)
                    .Include(r=>r.Rad)
                    .Where(r=>r.RadId == rad.Id)
                    .ToList();

                string naslovKnjige = rad.Naslov;
                

                foreach (var item in recenzentRad)
                {
                    string folderPath = Path.Combine(_environment.WebRootPath, "Uploads",
                       item.Recenzent.UserName, "Recenzije", naslovKnjige);

                   
                    System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                    //directory exists only if reviewer added the review file
                    if (di.Exists) { 
                        //get all files from subfolder
                        var fileList = System.IO.Directory.GetFiles(folderPath);

                        //check if directory contains any files
                        if(fileList.Length > 0) { 
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

                //do something
            }

            var autorRad = _context.AutorRad
                .Include(a => a.Author)
                .FirstOrDefault(a => a.RadId == model.Id);

            model.GlavniAutor = autorRad.Author.Title + " " + autorRad.Author.FirstName + " " + autorRad.Author.LastName;

            model.DocumentExists = _context.Document.Any(d => d.RadId == model.Id);

            return View(model);
        }

        [Authorize(Roles = "Author")]
        public IActionResult RadoviIndexByAuthor(string title = "", string type="", string status = "")
        {
            var currentUserId = _userManager.GetUserId(User);

            var radovi = _context.AutorRad.Include(r => r.Rad)
                .Include(a=>a.Rad)
                .Where(a => a.AuthorId == currentUserId)
                .Select(r => new RadoviIndexVM
                {
                    Id = r.RadId,
                    Naslov = r.Rad.Naslov,
                    OcjenaAdmina = r.Rad.ApprovedByAdmin,
                    TipRada = r.Rad.Tip,
                    PublishDate = r.Rad.DatumObjavljivanja,
                    OcjenaRecenzenta = r.Rad.ApprovedByAdmin,
                    UploadDate = r.Rad.UploadDate
                }).ToList();

            if (!String.IsNullOrEmpty(title))
                radovi = radovi.Where(r => r.Naslov.ToLower().Contains(title.ToLower())).ToList();

            if (type != "all")
                radovi = radovi.Where(r => r.TipRada.ToLower().Contains(type.ToLower())).ToList();

            if (status != "all")
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
                Radovi = radovi,
                TipoviRada = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "0",
                        Text = "Reviewed Paper"
                    },
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Student Paper"
                    },
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Idea"
                    },
                    new SelectListItem
                    {
                        Value = "3",
                        Text = "eBook"
                    }
                }
            };

            return View(model);
        }

        [Authorize(Roles = "Reviewer")]
        public IActionResult RadoviIndexByRecenzent(string title = "", string type = "", string status = "")
        {
            var currentUserId = _userManager.GetUserId(User);


            var radovi = _context.RecenzentRad
                .Include(r => r.Recenzent)
                .Include(r=>r.Rad)
                .Where(r => r.RecenzentId == currentUserId)
                .Select(r => new RadoviIndexVM
                {
                    Id = r.RadId,
                    Naslov = r.Rad.Naslov,
                    OcjenaRecenzenta = r.ApprovedByRecenzent,
                    OcjenaAdmina = r.Rad.ApprovedByAdmin,
                    TipRada = r.Rad.Tip,
                    UploadDate = r.Rad.UploadDate,
                    PublishDate = r.Rad.DatumObjavljivanja
                }).ToList();

            if (!String.IsNullOrEmpty(title))
                radovi = radovi.Where(r => r.Naslov.ToLower().Contains(title.ToLower())).ToList();

            if (type != "all")
                radovi = radovi.Where(r => r.TipRada.ToLower().Contains(type.ToLower())).ToList();

            if (status != "all")
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

        //koristi se za dodavanje novih radova u publikaciju
        public IActionResult GetByNaziv(string naziv, int publikacijaId)
        {
            var sviRadovi = _context.Rad.Where(r => r.PublikacijaId.HasValue == false);

            var radovi = sviRadovi.Where(r => r.Naslov.ToLower().Contains(naziv.ToLower())).ToList();

            List<RadoviIndexVM> titles = radovi.Select(r => new RadoviIndexVM
            {
                Id = r.Id,
                Naslov = r.Naslov,
                TipRada = r.Tip,
                OcjenaAdmina = r.ApprovedByAdmin
            }).ToList();

            var model = new RadoviIndexListVM
            {
                Radovi = titles,
                PublikacijaId = publikacijaId
            };

            return View(model);
        }

        public IActionResult PrikaziRad(int id)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            //var currentUser = _userManager.GetUserName(User);

            var autor = _context.AutorRad.Include(r => r.Author)
                .FirstOrDefault(r => r.RadId == id)
                .Author.UserName;

            //ovdje se pretpostavlja da postoji samo jedna verzija dokumenta
            
            var sveVerzije = _context.Document.Where(r => r.RadId == id);

            var file = sveVerzije.FirstOrDefault(v => v.TrenutnaVerzija == true);

            var fileName = file.FileName;
            var brojVerzije = file.Version;

            string filePath = "/Uploads/" + autor + "/" + rad.Naslov + "/" + "verzija_" + brojVerzije + "_" + fileName;

            var model = new PrikaziRadVM
            {
                Id = rad.Id,
                Naslov = rad.Naslov,
                FileName = filePath
            };

            return View(model);
        }

        /*
        public IActionResult DodajKomentar(int id)
        {
            var model = new DodajKomentarVM
            {
                RadId = id
            };

            return View(model);
        }
        */

        [HttpPost]
        public IActionResult DodajKomentar(string id, string sadrzaj) //bilo je int id string sadrzaj
        {
            if (String.IsNullOrEmpty(sadrzaj))
            {
                return RedirectToAction("RadDetails", new { @id = id });
            }

            var currentUserId = _userManager.GetUserId(User);

            if(currentUserId == null)
            {
                return RedirectToAction("RadDetails", new { @id = id });
            }

            int radId = Convert.ToInt32(id);

            _context.Add(new JavniKomentar
            {
                Datum = DateTime.Now,
                RadId = radId,
                ApplicationUserId = currentUserId, //id citaoca tj. autora komentara
                Sadrzaj = sadrzaj
            });

            _context.SaveChanges();

            return RedirectToAction("KomentariNaRadIndex", new { @id = radId });
        }

        public IActionResult UkloniKomentar(int id)
        {
            var komentar = _context.JavniKomentar.FirstOrDefault(k => k.Id == id);

            var radId = komentar.RadId;

            _context.JavniKomentar.Remove(komentar);
            _context.SaveChanges();

            return RedirectToAction("RadDetails", new { @id = radId });
        }

        public IActionResult UrediKomentar(int id)
        {
            if(!_context.JavniKomentar.Any(k=>k.Id == id))
            {
                return RedirectToAction("RadDetails", new { @id = id });
            }

            var komentar = _context.JavniKomentar.FirstOrDefault(k => k.Id == id);

            var model = new DodajKomentarVM
            {
                KomentarId = komentar.Id,
                Sadrzaj = komentar.Sadrzaj,
                Datum = komentar.Datum
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UrediKomentar(DodajKomentarVM model)
        {
            if (ModelState.IsValid) { 
                var komentar = _context.JavniKomentar.FirstOrDefault(k => k.Id == model.KomentarId);

                komentar.Sadrzaj = model.Sadrzaj;
                _context.SaveChanges();
            }

            return RedirectToAction("RadDetails", new { @id = model.RadId });
        }

        public IActionResult KomentariNaRadIndex(int id)
        {

            string naslovRada = _context.Rad.FirstOrDefault(r => r.Id == id).Naslov;

            var komentari = _context.JavniKomentar
                .Include(r=>r.ApplicationUser)
                .Where(k=>k.RadId == id)
                .Select(k => new KomentarNaRadVM
            {
                Id = k.Id,
                Datum = k.Datum.ToShortDateString(),
                Autor = k.ApplicationUser.UserName,
                RadId = k.RadId,
                Sadrzaj = k.Sadrzaj
            });

            var model = new KomentarNaRadIndexVM
            {
                RadId = id,
                Komentari = komentari,
                NaslovRada = naslovRada
            };
            
            return View(model);
        }

        public IActionResult PregledSvihVerzija(int id)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            var verzije = _context.Document.Where(r => r.RadId == id).Select(v=> new VerzijaIndexVM {
                Verzija = v.Version,
                FileName = v.FileName,
                VerzijaId = v.Id,
                TrenutnaVerzija = v.TrenutnaVerzija
            });

            var model = new VerzijaIndexListVM
            {
                RadId = rad.Id,
                NazivRada = rad.Naslov,
                Verzije = verzije
            };
            
            return View(model);
        }
    }
}