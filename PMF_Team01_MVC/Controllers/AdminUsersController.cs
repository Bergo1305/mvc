using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PMF_Team01_MVC.Data;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.AdminUsersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PMF_Team01_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(ApplicationDbContext context, 
            IHostingEnvironment environment, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserIndex(string username = "", string status = "", string role = "")
        {

            var allUsers = _context.Users.ToList();

            //Filtering users by role
            var filteredUsers = new List<ApplicationUser>();

            if (String.IsNullOrEmpty(role) || role == "All")
                filteredUsers = allUsers;
            else
            {
                foreach(var item in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(item, role))
                        filteredUsers.Add(item);
                }
            }

            List<UserIndexVM> users = new List<UserIndexVM>();

            foreach(var user in filteredUsers)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                users.Add(new UserIndexVM {
                    Id = user.Id,
                    Affiliation = user.Affiliation,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = isAdmin,
                    Username = user.UserName
                });
            }

            if (!String.IsNullOrEmpty(username))
                users = users.Where(u => u.Username.ToLower().Contains(username.ToLower())).ToList();

            if(!String.IsNullOrEmpty(status) && status != "Not Activated")
            {
                if(status == "Active")
                    users = users.Where(u => u.IsEnabled == true).ToList();
                else if(status == "Banned")
                    users = users.Where(u => u.IsEnabled == false).ToList();

            }

            //if (!String.IsNullOrEmpty(affiliation))
            //    users = users.Where(u => u.Affiliation.ToLower().Contains(affiliation.ToLower())).ToList();



            var model = new UserIndexListVM
            {
                Users = users
            };

            return View(model);
        }

        public IActionResult ActivateUser(string Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);

            user.IsEnabled = true;

            _context.SaveChanges();

            return RedirectToAction("UserDetails", new { @id = Id });
        }

        public IActionResult BanUser(string Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);
            user.IsEnabled = false;

            _context.SaveChanges();

            return RedirectToAction("UserDetails", new { @id = Id});
        }


        public async Task<IActionResult> UserDetails(string Id)
        {
            var user = _context.Users
                //.Include(u=>u.Grad.Drzava)
                //.Include(u=>u.Grad)
                .FirstOrDefault(u => u.Id == Id);


            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserDetailsVM
            {
                Id = user.Id,
                Email = user.Email,
                Initials = user.Initials,
                IsEnabled = user.IsEnabled,
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.Middlename,
                Affiliation = user.Affiliation,
                Roles = roles.ToList(),
                Grad = user.City,
                Drzava = user.Country
               // Grad = user.Grad.Naziv,
                //Drzava = user.Grad.Drzava.Naziv
            };
            

            var image = _context.Image.FirstOrDefault(u => u.ApplicationUserId == user.Id);

            if(image != null)
                model.ImagePath = "/Uploads/" + user.Email + "/" + image.FileName;
            
            //CV 
            string folderPath = Path.Combine(_environment.WebRootPath,
                        "Uploads", user.UserName);

            //get all files from directory
            var dir = new DirectoryInfo(folderPath);

            var files = dir.GetFiles();

            bool fileExists = files.Any(f => f.Name.Contains(".pdf"));

            if (fileExists)
            {
                var cvFile = files.FirstOrDefault(f => f.Name.Contains(".pdf"));
                model.CVPath = "/Uploads/" + user.UserName + "/" + cvFile.Name;
            }
            //get .pdf file -> cv je jedini fajl sa nastavkom .pdf
            //var file = files.FirstOrDefault(f => f.Name.Contains(".pdf"));

            //if (file.Exists)
            //    model.CVPath = "/Uploads/" + user.Email + "/" + file.Name;

            return View(model);
        }
        
        //Mozda uraditi kasnije...
        public IActionResult UserRoles(string userId)
        {
            return View();
        }

        public async Task<IActionResult> DodijeliAdmina(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (await _roleManager.RoleExistsAsync("Admin") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                });
            }

            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
            
            if(isInRole != true)
                await _userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction("UserIndex");
        }

        public async Task<IActionResult> UkloniAdmina(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser.Id != id) { 
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                if (isInRole == true)
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            return RedirectToAction("UserIndex");
        }
    }
}