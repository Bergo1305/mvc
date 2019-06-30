using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.PublikacijaViewModels;
using Microsoft.AspNetCore.Mvc;
using PMF_Team01_MVC.Models.RadViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMF_Team01_MVC.Controllers
{
    public class PublikacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublikacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string number = "")
        {
            IEnumerable<PublikacijaVM> publikacije = _context.Publikacija.Select(p => new PublikacijaVM
            {
                Id = p.Id,
                Napomena = p.Napomena,
                DatumIzdavanja = p.DatumIzdavanja,
                BrojPublikacije = p.BrojPublikacije
            }).ToList();

            if (!String.IsNullOrEmpty(number))
                publikacije = publikacije.Where(p => p.BrojPublikacije.ToLower().Contains(number.ToLower())).ToList();
            

            var model = new PublikacijaListVM
            {
                Publikacije = publikacije
            };

            return View(model);
        }

        //Published publications all users can see
        public IActionResult PublishedPublikacijeIndex(string number = "")
        {
            IEnumerable<PublikacijaVM> publikacije = _context.Publikacija
                .Where(pub=>pub.DatumIzdavanja.HasValue)
                .Select(p => new PublikacijaVM
            {
                Id = p.Id,
                Napomena = p.Napomena,
                DatumIzdavanja = p.DatumIzdavanja,
                BrojPublikacije = p.BrojPublikacije
            }).ToList();

            if (!String.IsNullOrEmpty(number))
                publikacije = publikacije.Where(p => p.BrojPublikacije.ToLower().Contains(number.ToLower())).ToList();


            var model = new PublikacijaListVM
            {
                Publikacije = publikacije
            };

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var publikacija = _context.Publikacija.FirstOrDefault(p => p.Id == id);

            var model = new PublikacijaVM
            {
                Id = publikacija.Id,
                DatumIzdavanja = publikacija.DatumIzdavanja,
                BrojPublikacije = publikacija.BrojPublikacije,
                DatumKreiranja = publikacija.DatumKreiranja,
                Napomena = publikacija.Napomena,
                ListaRadova = _context.Rad.Where(r=>r.PublikacijaId == publikacija.Id).Select(np=> new RadoviIndexVM
                {
                    Id = np.Id,
                    Naslov = np.Naslov,
                    TipRada = np.Tip,
                    PublishDate = np.DatumObjavljivanja
                }).ToList()
            };
            
            
            return View(model);
        }

        public IActionResult AddNew()
        {
            PublikacijaVM model = new PublikacijaVM();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddNew(PublikacijaVM model)
        {
            if (ModelState.IsValid) { 
            var newModel = new Publikacija
            {
                BrojPublikacije = model.BrojPublikacije,
                DatumKreiranja = DateTime.Now.Date,
                DatumIzdavanja = null, //datum izdavanja se rjesava kasnije
                Napomena = model.Napomena
            };

            _context.Publikacija.Add(newModel);
            _context.SaveChanges();

            return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult SearchTitles()
        {
            return View();
        }

        public IActionResult DodajRad(int radID, int publikacijaID)
        {
            _context.Rad.FirstOrDefault(r => r.Id == radID).PublikacijaId = publikacijaID;
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult DodajRad2(PublikacijaSelectVM model)
        {
            _context.Rad.FirstOrDefault(r => r.Id == model.radID).PublikacijaId = Convert.ToInt32(model.publikacijaID);
            _context.SaveChanges();

            return RedirectToAction("AdminRadDetails", "Admin", new { @id = model.radID });
        }

        public IActionResult UkloniRad(int radID, int publikacijaID)
        {
            var rad = _context.Rad.FirstOrDefault(r => r.Id == radID && r.PublikacijaId == publikacijaID);
            rad.PublikacijaId = null;

            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = publikacijaID });
        }

        public IActionResult Edit(int id)
        {
            var publikacija = _context.Publikacija.FirstOrDefault(p => p.Id == id);

            var model = new PublikacijaVM
            {
                Id = id,
                BrojPublikacije = publikacija.BrojPublikacije,
                Napomena = publikacija.Napomena,
                DatumIzdavanja = publikacija.DatumIzdavanja,
                DatumKreiranja = publikacija.DatumKreiranja
            };
            
            var radovi = _context.Rad.Where(r => r.PublikacijaId == model.Id).Select(rr => new RadoviIndexVM
            {
                Id = rr.Id,
                Naslov = rr.Naslov,
                TipRada = rr.Tip,
                OcjenaAdmina = rr.ApprovedByAdmin,
                PublishDate = rr.DatumObjavljivanja
            }).ToList();

            model.ListaRadova = radovi;

            return View(model);
        }

        [HttpPost]
        public IActionResult EditChanges(PublikacijaVM model)
        {
            var publikacija = _context.Publikacija.FirstOrDefault(p => p.Id == model.Id);
            publikacija.BrojPublikacije = model.BrojPublikacije;
            publikacija.Napomena = model.Napomena;
            _context.SaveChanges();


            return RedirectToAction("Edit", new { @id = model.Id });
        }

        public IActionResult ChangeStatus(int publikacijaID)
        {
            var publikacija = _context.Publikacija.FirstOrDefault(p => p.Id == publikacijaID);

            if (publikacija.DatumIzdavanja.HasValue)
                publikacija.DatumIzdavanja = null;
            else
                publikacija.DatumIzdavanja = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = publikacijaID });
        }

        public IActionResult SelectPublikacija(int radID)
        {
            var publikacije = _context.Publikacija.Select(p => new SelectListItem
            {
                Text = p.BrojPublikacije,
                Value = p.Id.ToString()
            }).ToList();

            var model = new PublikacijaSelectVM
            {
                radID = radID,
                Publikacije = publikacije
            };

            return View(model);
        }

        public IActionResult DeletePublikacija(int id)
        {
            var radoviPublikacije = _context.Rad.Where(r => r.PublikacijaId == id).ToList();

            foreach(var rad in radoviPublikacije)
            {
                rad.PublikacijaId = null;
            }

            var publikacija = _context.Publikacija.FirstOrDefault(p => p.Id == id);

            _context.Publikacija.Remove(publikacija);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}