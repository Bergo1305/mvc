using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.Admin;
using PMF_Team01_MVC.Models.AutorRadViewModels;
using PMF_Team01_MVC.Models.RadViewModels;
using PMF_Team01_MVC.Models.RecenzentViewModels;
using PMF_Team01_MVC.Models.ReviewerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PMF_Team01_MVC.Controllers
{
    
    public class RecenzentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public RecenzentController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        [Authorize(Roles = "Reviewer")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DodijeliRecenzenta(int id)
        {
            //Dohvatanje svih slobodnih recenzenata
            //Kasnije mozda uraditi trazenje recenzenata na osnovu zadatih oblasti

            var sviRecenzenti = await _userManager.GetUsersInRoleAsync("Reviewer");

            var slobodniRecenzenti = new List<RecenzentIndexVM>();
            var dodijeljeniRecenzenti = new List<RecenzentIndexVM>();

            var recenzentRad = _context.RecenzentRad.Where(r => r.RadId == id);

            bool slobodan = true;

            foreach (var recenzent in sviRecenzenti)
            {
                foreach (var recRad in recenzentRad)
                {
                    if (recRad.RecenzentId == recenzent.Id)
                    {
                        slobodan = false;
                    }
                }

                var recenzentOblasti = _context.OblastRecenzent
                    .Include(o => o.Oblast)
                    .Where(r => r.ApplicationUserId == recenzent.Id).ToList();


                List<string> oblastiList = new List<string>();
                foreach (var oblast in recenzentOblasti)
                {
                    oblastiList.Add(oblast.Oblast.Naziv);
                }

                if (slobodan == true) {
                    
                    
                    
                    //var oblastiList = new List<string>();
                    //if (recenzentOblasti.Any())
                    //{

                      //  oblastiList.Add("obl1");
                        //oblastiList.Add("obl2");
                    //}

                    slobodniRecenzenti.Add(new RecenzentIndexVM {
                        Id = recenzent.Id,
                        Username = recenzent.UserName,
                        FirstName = recenzent.FirstName,
                        LastName = recenzent.LastName,
                        Oblasti = oblastiList
                    });
                }
                else
                {
                    dodijeljeniRecenzenti.Add(new RecenzentIndexVM
                    {
                        Id = recenzent.Id,
                        Username = recenzent.UserName,
                        FirstName = recenzent.FirstName,
                        LastName = recenzent.LastName,
                        Oblasti = oblastiList
                    });
                    
                    slobodan = true;
                }
            }

            

            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            var model = new DodijeliRecenzentaVM
            {
                RadId = id,
                NazivRada = rad.Naslov,
                Recenzenti = slobodniRecenzenti,
                DodijeljeniRecenzenti = dodijeljeniRecenzenti
            };

            return View(model);
        }


        public IActionResult DodijeliRecenzentaSave(int radId, string recenzentId)
        {
            _context.Add(new RecenzentRad
            {
                RadId = radId,
                RecenzentId = recenzentId,
                ApprovedByRecenzent = false
            });

            _context.SaveChanges();

            //return RedirectToAction("AdminRadoviIndex", "Admin");
            return RedirectToAction("DodijeliRecenzenta", new { @id = radId });
        }

        public IActionResult OcijeniRadRecenzent(int id)
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
                //Naslov = rad.Naslov,
                RadId = rad.Id,
                Ocjene = ocjene
            };

            return View(model);
        }

        [Authorize(Roles = "Reviewer")]
        [HttpPost]
        public IActionResult OcijeniRadRecenzent(int radID, string ocjena) //original: OcijeniRadVM model
        {
            var currentUserId = _userManager
                .GetUserId(User);

            //var radId = model.RadId;
            var radId = radID;

            var radRecenzent = _context.RecenzentRad
                .Include(r => r.Rad)
                .FirstOrDefault(r => r.RadId == radId && r.RecenzentId == currentUserId);


            _context.Update(radRecenzent);

            switch (ocjena)
            {
                case "-1":
                    if (radRecenzent.ApprovedByRecenzent.HasValue && radRecenzent.ApprovedByRecenzent.Value == true)
                        radRecenzent.Rad.BrojPozitivnihOcjena -= 1;

                    radRecenzent.ApprovedByRecenzent = null;
                    break;

                case "0":
                    if (radRecenzent.ApprovedByRecenzent.HasValue && radRecenzent.ApprovedByRecenzent.Value == true)
                        radRecenzent.Rad.BrojPozitivnihOcjena = -1;

                        radRecenzent.ApprovedByRecenzent = false;
                    break;

                case "1":
                    radRecenzent.ApprovedByRecenzent = true;

                    radRecenzent.Rad.BrojPozitivnihOcjena += 1;
                    break;
            }

            _context.SaveChanges();

            return RedirectToAction("EditRadRecenzent", new { @id = radId });
            
        }

        [Authorize(Roles = "Reviewer")]
        public IActionResult EditRadRecenzent(int id)
        {

            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            string recenzentId = _userManager.GetUserId(User);

            var radRecenzent = _context.RecenzentRad.FirstOrDefault(r => r.RadId == id && recenzentId == r.RecenzentId);

            var model = new EditRadRecenzentVM
            {
                Id = rad.Id,
                Apstrakt = rad.Apstrakt,
                ApprovedByAdmin = rad.ApprovedByAdmin,
                Naslov = rad.Naslov,
                MojaOcjena = radRecenzent.ApprovedByRecenzent,
                PublishDate = rad.DatumObjavljivanja,
                KeyWords = rad.KeyWords,
                TipRada = rad.Tip,
                UploadDate  = rad.UploadDate
            };

            var oblasti = _context.OblastRad
                .Include(o=>o.Oblast)
                .Where(o => o.RadId == rad.Id);

            model.Oblasti = new List<string>();

            foreach(var item in oblasti)
            {
                model.Oblasti.Add(item.Oblast.Naziv);
            }

            if(rad.Tip == "Recenzirani")
            {
                model.TipRecenziranogRada = _context.RecenziraniRad.FirstOrDefault(r => r.RecenziraniRadId == model.Id).TipRecenziranogRada;
            }
            else if(rad.Tip == "Ideja")
            {
                model.TekstIdeje = _context.Ideja.FirstOrDefault(i => i.IdejaId == model.Id).TekstIdeje;

                var kategorije = _context.KategorijaIdeja.Include(k => k.Kategorija)
                    .Where(k => k.IdejaId == rad.Id);

                model.Kategorije = new List<string>();

                foreach(var item in kategorije)
                {
                    model.Kategorije.Add(item.Kategorija.Naziv);
                }
            }
            model.DocumentExists = _context.Document.Any(d => d.RadId == model.Id);

            return View(model);
        }
        
        [Authorize(Roles = "Reviewer")]
        public IActionResult AddRecenzija(int id)
        {
            if(!_context.Rad.Any(r=>r.Id == id))
            {
                return RedirectToAction("EditRadRecenzent", new { @id = id });
            }

            //id - id knjige
            var recenzentid = _userManager.GetUserId(User);

            var model = new AddRecenzijaVM
            {
                EKnjigaId = id,
                RecenzentId = recenzentid
            };

            return View(model);
        }

        [Authorize(Roles = "Reviewer")]
        [HttpPost]
        public async Task<IActionResult> AddRecenzija(AddRecenzijaVM model)
        {
            if (ModelState.IsValid) {

                if (Path.GetExtension(model.File.FileName).Equals(".pdf"))
                {

                 
            var currentUser = await _userManager.GetUserAsync(User);
            
            var recenzija = new Recenzija
            {
                EKnjigaId = model.EKnjigaId,
                ReviewerId = model.RecenzentId,
                FileName = model.File.FileName,
                Version = "0"
            };

            var naslovKnjige = _context.EKnjiga.Include(e => e.Rad).FirstOrDefault(e => e.EKnjigaId == model.EKnjigaId).Rad.Naslov;
            
            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads",
                   currentUser.UserName, "Recenzije", naslovKnjige);
            
            System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

            if(!di.Exists)
                Directory.CreateDirectory(folderPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); //remove all files from the directory
            }

            var filePath = Path.Combine(folderPath, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }
            }
            }
            return RedirectToAction("EditRadRecenzent", new { @id = model.EKnjigaId });
        }

        //dovrsiti kasnije...
        [Authorize(Roles = "Reviewer")]
        public IActionResult DodijeliRecenziju(int id)
        {
            var eKnjiga = _context.EKnjiga
                .Include(k => k.Rad)
                .FirstOrDefault(k => k.EKnjigaId == id);



            return View();
        }

        public IActionResult OblastiIndex(string recenzentId)
        {
            //Index svih oblasti koje trenutno nisu povezane sa recenzentom

            var sveOblasti = _context.Oblast.ToList();

            var oblastRecenzent = _context.OblastRecenzent
                .Where(r => r.ApplicationUserId == recenzentId)
                .ToList();

            var slobodneOblastiList = new List<OblastIndexVM>();
            var recenzentoveOblastiList = new List<OblastIndexVM>();

            foreach(var oblast in sveOblasti)
            {
                if (!oblastRecenzent.Any(o => o.OblastId == oblast.Id))
                {
                    slobodneOblastiList.Add(new OblastIndexVM
                    {
                        Id = oblast.Id,
                        Naziv = oblast.Naziv
                    });
                }
                else
                {
                    recenzentoveOblastiList.Add(new OblastIndexVM
                    {
                        Id = oblast.Id,
                        Naziv = oblast.Naziv
                    });
                }
            }



            var recenzent = _context.Users.FirstOrDefault(u => u.Id == recenzentId);

            RecenzentManageOblastiVM model = new RecenzentManageOblastiVM
            {
                RecenzentId = recenzentId,
                RecenzentName = recenzent.FirstName + " " + recenzent.LastName,
                SlobodneOblasti = slobodneOblastiList,
                RecenzentOblasti = recenzentoveOblastiList
            };


            //if (!slobodneOblastiList.Any())
              //  return RedirectToAction("UserDetails", "AdminUsers", new { @Id = recenzentId });

            return View(model);
        }

        public IActionResult DodijeliOblastRecenzentu(string recenzentId, int oblastId)
        {
            _context.OblastRecenzent.Add(new OblastRecenzent
            {
                ApplicationUserId = recenzentId,
                OblastId = oblastId
            });

            _context.SaveChanges();

            return RedirectToAction("OblastiIndex", new { @recenzentId = recenzentId });
        }

        public IActionResult UkloniOblastRecenzentu(string recenzentId, int oblastId)
        {
            var oblastRec = _context.OblastRecenzent.FirstOrDefault(r => r.ApplicationUserId == recenzentId && r.OblastId == oblastId);

            _context.OblastRecenzent.Remove(oblastRec);
            _context.SaveChanges();

            return RedirectToAction("OblastiIndex", new { @recenzentId = recenzentId });
        }

        //ukloni recenzenta od rada kojem je dodijeljen
        public IActionResult UkloniRecenzenta(string recenzentId, int radId)
        {
            var recenzentRad = _context.RecenzentRad.FirstOrDefault(r => r.RadId == radId && r.RecenzentId == recenzentId);

            var rad = _context.Rad.FirstOrDefault(r => r.Id == radId);

            //ako je recenzent pozitivno ocijenio rad, smanji broj pozitivnih ocjena...
            if(recenzentRad.ApprovedByRecenzent == true)
            {
                rad.BrojPozitivnihOcjena -= 1;
            }

            _context.RecenzentRad.Remove(recenzentRad);
            _context.SaveChanges();


            return RedirectToAction("DodijeliRecenzenta", new { id = radId });
        }
    }
}
