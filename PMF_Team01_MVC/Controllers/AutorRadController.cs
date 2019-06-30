using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.AuthorTitles;
using PMF_Team01_MVC.Models.AutorRadViewModels;
using PMF_Team01_MVC.Models.RadViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PMF_Team01_MVC.Controllers
{
    [Authorize(Roles = "Author,Reader")]
    public class AutorRadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AutorRadController(ApplicationDbContext context,
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


        /*
         na pocetku se bira tip rada koji se dodaje:
         1 - recenzirani rad
         2 - studentski
         3 - ideja
         4 - eknjiga
             */

        [Authorize(Roles = "Author")]
        public IActionResult AddRad(string tip)
        {
            var model = new AddRadVM();



            if (tip == "0")
            {
                model.TipRada = "Recenzirani";

                model.TipRecenziranogRadaList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "0",
                        Text = "Conference Style"
                    },
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Magazine Style"
                    }
                };

            }
            else if (tip == "1")
            {
                model.TipRada = "Studentski";

                model.MentoriList = _context.Mentor.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Ime + " " + m.Prezime
                }).ToList();

                model.TipStudentskogRadaList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Seminarski"
                    },
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Maturski"
                    }
                };
            }
            else if (tip == "2")
            {
                model.TipRada = "Ideja";
            }
            else
            {
                model.TipRada = "EKnjiga";
            }


            return View(model);
        }

        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                    string.Empty;
        }

        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> AddRad(AddRadVM model)
        {
            string tip = model.TipRada;

            if (ModelState.IsValid)
            {
                if(model.TipRada == "Recenzirani" 
                    || model.TipRada == "Studentski" 
                    || model.TipRada == "EKnjiga")
                {
                    if(model.Document == null || String.IsNullOrEmpty(model.Document.FileName))
                    {
                        ModelState.AddModelError(string.Empty, "Document is required!");
                        return View(model);
                    }
                }
                
                        var currentUserId = _userManager.GetUserId(User);

                        var rad = new Rad
                        {
                            Naslov = model.Naslov,
                            Apstrakt = model.Apstrakt,
                            ApprovedByAdmin = null,
                            DatumObjavljivanja = null,
                            Tip = model.TipRada,
                            BrojPozitivnihOcjena = 0,
                            KeyWords = model.KeyWords,
                            UploadDate = DateTime.Now,
                            OstaliAutori = model.OstaliAutori
                        };

                        _context.Add(rad);

                        _context.Add(new AutorRad
                        {
                            RadId = rad.Id,
                            AuthorId = currentUserId
                        });

                        _context.SaveChanges();

                        var currentUser = _context.Users.FirstOrDefault(u => u.Id == currentUserId);

                        var brojVerzija = 1;

                //Dodaje se novi dokument
                //Nova verzija dokumenta se postavlja kao trenutna ili zadana verzija
                //koja se kasnije, ako se rad prihvati
                //prikazuje korisnicima

                if (model.Document != null && !String.IsNullOrEmpty(model.Document.FileName))
                {
                    //sta ako je model.document == null???
                    string ext = Path.GetExtension(model.Document.FileName);
                

                var noviDoc = new Document
                {
                    RadId = rad.Id,
                    FileName = model.Document.FileName,
                    Version = brojVerzija.ToString(),
                    TrenutnaVerzija = true
                };

                        _context.Add(noviDoc);

                        _context.SaveChanges();

                        string folderPath = Path.Combine(_environment.WebRootPath, "Uploads",
                            currentUser.UserName, model.Naslov);

                        AddQuotesIfRequired(folderPath);

                        Directory.CreateDirectory(folderPath);


                        var filePath = Path.Combine(folderPath, "verzija_"
                            + noviDoc.Version.ToString() + "_" + model.Document.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Document.CopyToAsync(stream);
                        }
                }
                //naknadno dodano
                if (model.TipRada == "Recenzirani")
                        {
                            string tipRecRada = " ";

                            if (model.TipRecenziranogRada == "1")
                                tipRecRada = "Conference Style";
                            else
                                tipRecRada = "Magazine Style";

                            _context.Add(new RecenziraniRad
                            {
                                RecenziraniRadId = rad.Id,
                                TipRecenziranogRada = tipRecRada
                            });
                        }
                        else if (model.TipRada == "Studentski")
                        {
                            var newStudentski = new StudentskiRad
                            {
                                StudentskiRadId = rad.Id,
                                MentorId = Convert.ToInt32(model.Mentor)
                            };

                            if (model.TipStudentskogRada == "1")
                                newStudentski.TipStudentskogRada = "Seminarski";
                            else
                                newStudentski.TipStudentskogRada = "Diplomski";

                            _context.Add(newStudentski);


                        }
                        else if (model.TipRada == "Ideja")
                        {
                            _context.Add(new Ideja
                            {
                                IdejaId = rad.Id,
                                TekstIdeje = model.TekstIdeje
                            });
                            _context.SaveChanges();

                            //return RedirectToAction("DodajKategorijuIndex", new { @id = rad.Id });
                        }
                        else if (model.TipRada == "EKnjiga")
                        {
                            _context.Add(new EKnjiga
                            {
                                EKnjigaId = rad.Id
                            });
                        }

                        _context.SaveChanges();

                        int radId = rad.Id;


                        return RedirectToAction("RadoviIndexByAuthor", "Rad");
                    }


            model.TipRada = tip;

            if(model.TipRada == "Recenzirani")
            {
                model.TipRecenziranogRadaList = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Text = "Conference Style",
                        Value = "0"
                    },
                    new SelectListItem
                    {
                        Text = "Magazine Style",
                        Value = "1"
                    }
                };
            }

            else if(model.TipRada == "Studentski")
            {
                model.MentoriList = _context.Mentor.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Ime + " " + m.Prezime
                }).ToList();

                model.TipStudentskogRadaList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Seminarski"
                    },
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Maturski"
                    }
                };
            }

            return View(model);
        }

        //NAPRAVITI METODU ZA DODAVANJE NOVE VERZIJE NEKOG RADA
        //ODNOSNO UPDATE RAD
        [Authorize(Roles = "Author")]
        public IActionResult UpdateVerzijeRada(int id)
        {

            if (_context.Rad.Any(r=>r.Id == id) == false)
                return RedirectToAction("EditRad", new { @id = id});

            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            if (rad.ApprovedByAdmin.HasValue && rad.ApprovedByAdmin.Value == false)
            {
                //ako approvedByAdmin ima vrijednost znaci da je rad ili prihvacen ili odbijen
                //ovo mozda mijenjati kasnije
                return RedirectToAction("RadoviIndexByAuthor", "Rad");
            }

            var model = new UpdateVerzijeRadaVM
            {
                RadId = id,
                NazivRada = rad.Naslov
            };

            return View(model);
        }

        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> UpdateVerzijeRada(UpdateVerzijeRadaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ext = Path.GetExtension(model.Document.FileName);

            if (!ext.Equals(".pdf"))
            {
                ModelState.AddModelError(string.Empty, "Uploaded file does not have .pdf extension!");
                return View(model);
            }

            int verzijeCount = 0;
            
            var sveVerzije = _context.Document.Where(d => d.RadId == model.RadId);
            verzijeCount = sveVerzije.Count();

            verzijeCount += 1;

            var currentUserId = _userManager.GetUserId(User);
            var currentUserName = _userManager.GetUserName(User);
            
            var novaVerzija = new Document
            {
                RadId = model.RadId,
                FileName = model.Document.FileName,
                TrenutnaVerzija = true,
                Version = verzijeCount.ToString()
            };

            foreach (var verzija in sveVerzije)
            {
                verzija.TrenutnaVerzija = false;
            }

            //UPLOAD FAJLA

            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads",
                 currentUserName, model.NazivRada);

            //Kreiraj folder. Ako folder vec postoji nece biti kreiran.
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, "verzija_" + novaVerzija.Version.ToString() + "_" + model.Document.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Document.CopyToAsync(stream);
            }

            _context.Add(novaVerzija);
            _context.SaveChanges();

            return RedirectToAction("PregledSvihVerzija", "Rad", new { id = model.RadId });
        }

        [Authorize(Roles = "Author")]
        public IActionResult DodajOblastIndex(int id)
        {
            //sve oblasti
            var sveOblasti = _context.Oblast.ToList();

            //svi oblastRadovi koji se veze za Rad
            var oblastRadovi = _context.OblastRad.Where(o => o.RadId == id);

            var slobodneOblasti = new List<OblastIndexVM>();
            var oblastiRada = new List<OblastIndexVM>();
          
            foreach(var item in sveOblasti)
            {
                if(!oblastRadovi.Any(o=>o.OblastId == item.Id))
                {
                    slobodneOblasti.Add(new OblastIndexVM
                    {
                        Id = item.Id,
                        Naziv = item.Naziv
                    });
                }
                else
                {
                    oblastiRada.Add(new OblastIndexVM
                    {
                        Id = item.Id,
                        Naziv = item.Naziv
                    });
                }
            }
          

            var model = new DodajOblastIndex
            {
                RadId = id,
                Oblasti = slobodneOblasti,
                OblastiRada = oblastiRada
            };

            return View(model);
        }

        [Authorize(Roles = "Author")]
        public IActionResult DodijeliOblastRadu(int rad, int oblast)
        {
            _context.Add(new OblastRad
            {
                OblastId = oblast,
                RadId = rad
            });

            _context.SaveChanges();


            //return View();

            int radId = rad;

            return RedirectToAction("DodajOblastIndex", new { @id = radId });
        }
        
        public IActionResult UkloniOblastRadu(int radId, int oblastId)
        {
            var oblastRad = _context.OblastRad.FirstOrDefault(o => o.OblastId == oblastId && o.RadId == radId);

            _context.OblastRad.Remove(oblastRad);
            _context.SaveChanges();

            return RedirectToAction("DodajOblastIndex", new { @id = radId });
        }


        public IActionResult AutorProfile(string id)
        {
            var autor = _context.Users
                //.Include(a=>a.Grad)
                //.Include(a=>a.Grad.Drzava)
                .FirstOrDefault(u => u.Id == id);

            var model = new AutorProfileVM
            {
                Id = autor.Id,
                Ime = autor.FirstName,
                Prezime = autor.LastName,
                Email = autor.Email,
                Affiliation = autor.Affiliation,
                DatumRodjenja = autor.DateOfBirth,
                PhoneNumber = autor.PhoneNumber,
                Mjesto = autor.Country + " " + autor.State + " " + autor.City
            };

            var image = _context.Image.FirstOrDefault(u => u.ApplicationUserId == autor.Id);

            if(image != null)
                model.ImagePath = "/Uploads/" + autor.Email + "/" + image.FileName;
            
            //CV 
            string folderPath = Path.Combine(_environment.WebRootPath,
                        "Uploads", autor.UserName);

            //get all files from directory
            var dir = new DirectoryInfo(folderPath);

            var files = dir.GetFiles();

            //get .pdf file -> cv je jedini fajl sa nastavkom .pdf
            //var file = files.FirstOrDefault(f => f.Name.Contains(".pdf"));

            bool fileExists = files.Any(f => f.Name.Contains(".pdf"));

            if (fileExists)
            {
                var cvFile = files.FirstOrDefault(f => f.Name.Contains(".pdf"));
                model.CVPath = "/Uploads/" + autor.UserName + "/" + cvFile.Name;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AutoriIndex(string name = "")
        {
            //dohvatiti sve autore, tj. appusers gdje je role == "Author"
            //ovaj dio koda sluzi za testiranje i dohvatit ce sve korisnike

            var listaAutora = await _userManager.GetUsersInRoleAsync("Author");

            if (!String.IsNullOrEmpty(name))
            {
                listaAutora = listaAutora.Where(a => (a.FirstName.ToLower() + " " + a.LastName.ToLower()).Contains(name.ToLower())).ToList();
            }

            var autori = listaAutora.Select(a => new AutoriIndexVM
            {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName,
                UserName = a.UserName,
                Affiliation = a.Affiliation
            }).ToList();


            var model = new AutoriIndexListVM
            {
                Autori = autori
            };

            return View(model);
        }

        
        public IActionResult ListaRadova(string id)
        {
            var radovi = _context.AutorRad
                .Include(a => a.Rad)
                .Where(a => a.AuthorId == id && a.Rad.ApprovedByAdmin.HasValue)
                .Select(a => new RadoviIndexVM
                {
                    Id = a.Rad.Id,
                    Naslov = a.Rad.Naslov
                }).ToList();
            var model = new RadoviIndexListVM
            {
                Radovi = radovi
            };

            return View(model);
        }

        [Authorize(Roles = "Author")]
        public IActionResult OznaciKaoTrenutnu(int id)
        {

            var verzija = _context.Document.FirstOrDefault(d => d.Id == id);

            var radId = verzija.RadId;

            var sveVerzije = _context.Document.Where(d => d.RadId == radId);
            foreach (var v in sveVerzije)
            {
                v.TrenutnaVerzija = false;
            }

            verzija.TrenutnaVerzija = true;

            _context.SaveChanges();

            return RedirectToAction("PregledSvihVerzija", "Rad", new { @id = verzija.RadId });
        }

        [Authorize(Roles = "Author")]
        public IActionResult EditRad(int id)
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

            var model = new EditRadVM
            {
                RadId = rad.Id,
                Naslov = rad.Naslov,
                Apstrakt = rad.Apstrakt,
                IsOdobren = rad.ApprovedByAdmin,
                Oblasti = ListaOblasti,
                TipRada = rad.Tip,
                DatumObjavljivanja = rad.DatumObjavljivanja,
                KeyWords = rad.KeyWords,
                UploadDate = rad.UploadDate,
                ListaKategorijaOblasti = new List<string>(),
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
                    .Include(s => s.Mentor)
                    .FirstOrDefault(s=>s.StudentskiRadId == rad.Id);

                model.TipStudentskogRada = studentski.TipStudentskogRada;
                model.Mentor = studentski.Mentor.Ime + " " + studentski.Mentor.Prezime;
            }
            else if (rad.Tip == "Ideja")
            {
                var ideja = _context.Ideja.FirstOrDefault(i => i.IdejaId == rad.Id);
                model.TekstIdeje = ideja.TekstIdeje;
            }
            else if (rad.Tip == "EKnjiga")
            {
                //do something
            }

            if(rad.Tip != "Ideja")
            {
                var list = _context.OblastRad
                    .Include(o=>o.Oblast)
                    .Where(o=>o.RadId == model.RadId)
                    .ToList();

                foreach(var item in list)
                {
                    model.ListaKategorijaOblasti.Add(item.Oblast.Naziv);
                }
            }
            else
            {
                var list = _context.KategorijaIdeja
                    .Include(k => k.Kategorija)
                    .Where(k => k.IdejaId == model.RadId)
                    .ToList();

                foreach(var item in list)
                {
                    model.ListaKategorijaOblasti.Add(item.Kategorija.Naziv);
                }
            }

            var autorRad = _context.AutorRad
                .Include(a => a.Author)
                .FirstOrDefault(a => a.RadId == model.RadId);

            model.GlavniAutor = autorRad.Author.Title + " " + autorRad.Author.FirstName + " " + autorRad.Author.LastName;

            model.DocumentExists = _context.Document.Any(d => d.RadId == model.RadId);

            return View(model);
        }

        [Authorize(Roles = "Author")]
        [HttpPost]
        public IActionResult EditRad(int id, string keywords, string apstrakt, string argument = "", string otherAuthors = "") //original EditRadVM model
        {
            //if(String.IsNullOrEmpty(keywords) || String.IsNullOrEmpty(apstrakt))
            //{
              //  return RedirectToAction("EditRad", new { @id = id });
            //}

            if (!_context.Rad.Any(r => r.Id == id))
                return RedirectToAction("Index", "Home");

            var rad = _context.Rad.FirstOrDefault(r => r.Id == id);

            rad.Apstrakt = apstrakt;
            rad.KeyWords = keywords;
            rad.OstaliAutori = otherAuthors;

            if (rad.Tip == "Ideja") { 
                if(String.IsNullOrEmpty(argument))
                    return RedirectToAction("EditRad", new { @id = id });

                _context.Ideja.FirstOrDefault(i => i.IdejaId == rad.Id).TekstIdeje = argument;
            }
            _context.SaveChanges();

            return RedirectToAction("RadoviIndexByAuthor", "Rad");
        }

        //promijeni tip studentskog rada
        [Authorize(Roles = "Author")]
        public IActionResult ChangeType(int id, string type)
        {
            var studentskiRad = _context.StudentskiRad.FirstOrDefault(s => s.StudentskiRadId == id);

            if (type == "1")
                studentskiRad.TipStudentskogRada = "Type 1";
            else
                studentskiRad.TipStudentskogRada = "Type 2";

            _context.SaveChanges();

            return RedirectToAction("EditRad", new { @id = studentskiRad.StudentskiRadId });
        }
        [Authorize(Roles = "Author")]
        public IActionResult ChangeMentor(int id)
        {
            var mentori = _context.Mentor.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Ime + " " + m.Prezime
            }).ToList();

            var model = new ChangeMentorVM
            {
                RadId = id,
                MentorList = mentori
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Author")]
        public IActionResult ChangeMentorSave(int radId, string mentor)
        {
            var rad = _context.StudentskiRad.FirstOrDefault(r => r.StudentskiRadId == radId);
            rad.MentorId = Convert.ToInt32(mentor);

            _context.SaveChanges();

            return RedirectToAction("EditRad", new { @id = radId });
        }

        [Authorize(Roles = "Author")]
        public IActionResult DodajKategorijuIndex(int id)
        {

            var sveKategorije = _context.Kategorija.ToList();

            var kategorijaIdeja = _context.KategorijaIdeja.ToList();

            var slobodneKategorije = new List<KategorijaIndexVM>();
            var idejaKategorije = new List<KategorijaIndexVM>();

            foreach(var kategorija in sveKategorije)
            {
                if(!kategorijaIdeja.Any(k=>k.KategorijaId == kategorija.Id))
                {
                    slobodneKategorije.Add(new KategorijaIndexVM
                    {
                        Id = kategorija.Id,
                        Naziv = kategorija.Naziv
                    });
                }
                else
                {
                    idejaKategorije.Add(new KategorijaIndexVM
                    {
                        Id = kategorija.Id,
                        Naziv = kategorija.Naziv
                    });
                }
            }

            var model = new DodajKategorijuIndexVM
            {
                IdejaId = id,
                KategorijeIdeje = idejaKategorije,
                Kategorije = slobodneKategorije
            };

            return View(model);
        }

        [Authorize(Roles = "Author")]
        public IActionResult DodijeliKategorijuIdeji(int idejaId, int kategorijaId)
        {
            _context.Add(new KategorijaIdeja
            {
                IdejaId = idejaId,
                KategorijaId = kategorijaId
            });

            _context.SaveChanges();

            return RedirectToAction("DodajKategorijuIndex", new { @id = idejaId });
        }

        public IActionResult UkloniKategorijuIdeji(int idejaId, int kategorijaId)
        {
            var idejaKategorija = _context.KategorijaIdeja.FirstOrDefault(i => i.IdejaId == idejaId && i.KategorijaId == kategorijaId);
            _context.KategorijaIdeja.Remove(idejaKategorija);
            _context.SaveChanges();

            return RedirectToAction("DodajKategorijuIndex", new { @id = idejaId });

        }

        public IActionResult DeleteRad(int radId)
        {
            var radRecenzent = _context.RecenzentRad.Where(r => r.RadId == radId).ToList();
            var radOblast = _context.OblastRad.Where(r => r.RadId == radId).ToList();
            var radAutor = _context.AutorRad.Where(r => r.RadId == radId).ToList();

            foreach(var r in radRecenzent)
            {
                _context.RecenzentRad.Remove(r);
            }

            foreach(var r in radOblast)
            {
                _context.OblastRad.Remove(r);
            }

            foreach(var r in radAutor)
            {
                _context.AutorRad.Remove(r);
            }

            var rad = _context.Rad.FirstOrDefault(r => r.Id == radId);
            
            var currentUserId = _userManager.GetUserId(User);
            var autor = _context.Users.FirstOrDefault(u => u.Id == currentUserId);


            string folderPath = Path.Combine(_environment.WebRootPath, "Uploads",
                    autor.UserName, rad.Naslov);

            var dir = new DirectoryInfo(folderPath);
            dir.Delete(true);


            _context.Rad.Remove(rad);

            _context.SaveChanges();

            return RedirectToAction("RadoviIndexByAuthor", "Rad");
        }
    }
}