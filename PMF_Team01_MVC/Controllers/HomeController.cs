using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PMF_Team01_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models.RadViewModels;

namespace PMF_Team01_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(SignInManager<ApplicationUser> signInManager, 
            ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index2");

            return View();
        }


        //Index displayed to the user after signing in...
        public IActionResult Index2()
        {
            //get 5 newest published titles
            var radovi = _context.Rad
                .Where(r=>r.DatumObjavljivanja.HasValue)
                .OrderByDescending(r=>r.DatumObjavljivanja)
                .Take(5)
                .Select(r => new RadoviIndexVM
                {
                    Id = r.Id,
                    Naslov = r.Naslov,
                    TipRada = r.Tip,
                    PublishDate = r.DatumObjavljivanja
                })
                .ToList();

            var model = new RadoviIndexListVM
            {
                Radovi = radovi
            };
            
            return View(model);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Reader, Admin")]
        public IActionResult ReaderIndex()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
