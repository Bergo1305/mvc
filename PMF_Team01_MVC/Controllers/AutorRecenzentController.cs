using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.AutorRecenzent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PMF_Team01_MVC.Controllers
{
    //kontroler za upravljanje komunikacijom izmedju autora i recenzenta
    //DODATI DATETIME U PRIVATNIKOMENTAR
    public class AutorRecenzentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public AutorRecenzentController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DodajPrivatniKomentar(string radId, string isAuthor)
        {
            //id = radId

            //trenutno logirani user, bilo autor ili recenzent
            //ce biti autor komentara

            int id = Convert.ToInt32(radId);
            var isAuthorInt = Convert.ToInt32(isAuthor);

            var autorId = _userManager.GetUserId(User);

            if(autorId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new DodajPrivatniKomentarVM
            {
                AutorId = autorId,
                RadId = id,
                IsAutor = isAuthorInt
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DodajPrivatniKomentar(DodajPrivatniKomentarVM model)
        {
            if (ModelState.IsValid) { 
            var autorId = _userManager.GetUserId(User);

            if(autorId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var komentar = new PrivatniKomentar
            {
                RadId = model.RadId,
                Sadrzaj = model.Sadrzaj,
                AutorKomentaraId = autorId
                //AutorKomentaraId = model.AutorId
            };

            _context.Add(komentar);
            _context.SaveChanges();

            
            //Dodavanje dokumenta
            //.../Uploads/KomentarDocs/11 - samo primjer

            if(model.Dokument != null) { 
                string folderPath = Path.Combine(_environment.WebRootPath, "Uploads", "KomentarDocs", komentar.Id.ToString());

            Directory.CreateDirectory(folderPath);

            string docPath = Path.Combine(folderPath, model.Dokument.FileName);

            using (var stream = new FileStream(docPath, FileMode.Create))
            {
                 await model.Dokument.CopyToAsync(stream);
            }

            _context.Add(new KomentarDokument
            {
                KomentarDokumentId = komentar.Id,
                FileName = model.Dokument.FileName
            });

            _context.SaveChanges();
            }
            }
            if (model.IsAutor == 1)
                return RedirectToAction("EditRad", "AutorRad", new { @id = model.RadId });

            else 
                return RedirectToAction("EditRadRecenzent", "Recenzent", new { @id = model.RadId });
        }
        
        public IActionResult PrivatniKomentariIndex(string radId, string isAuthor)
        {
            int id = Convert.ToInt32(radId);
            var privatniKomentari = _context.PrivatniKomentar
                .Where(k => k.RadId == id);

            var listaKomentara = privatniKomentari
                .Include(k=>k.KomentarDokument)
                .Select(k => new PrivatniKomentariIndexVM
            {
                AutorId = k.AutorKomentaraId,
                Autor = k.AutorKomentara.UserName,
                KomentarId = k.Id,
                Sadrzaj = k.Sadrzaj,
                FilePath = "/Uploads/KomentarDocs/" + k.Id + "/" + _context.KomentarDokument.FirstOrDefault(d=>d.KomentarDokumentId == k.Id).FileName
            }).ToList();

            var model = new PrivatniKomentariIndexListVM
            {
                RadId = id,
                Komentari = listaKomentara,
                IsAuthor = Convert.ToInt32(isAuthor)
            };

            return View(model);
        }

        public IActionResult UrediKomentar(int komentarId)
        {
            var komentar = _context.PrivatniKomentar.FirstOrDefault(k => k.Id == komentarId);

            var model = new DodajPrivatniKomentarVM
            {
                RadId = komentar.RadId,
                KomentarId = komentar.Id,
                Sadrzaj = komentar.Sadrzaj
            };

            return View(model);
        }
        
        [HttpPost]
        public IActionResult UrediKomentar(DodajPrivatniKomentarVM model)
        {
            var komentar = _context.PrivatniKomentar.FirstOrDefault(k => k.Id == model.KomentarId);

            komentar.Sadrzaj = model.Sadrzaj;
            _context.SaveChanges();

            return RedirectToAction(""); //??
        }
    }
}