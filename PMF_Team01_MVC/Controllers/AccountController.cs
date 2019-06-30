using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMF_Team01_MVC.Models;
using PMF_Team01_MVC.Models.AccountViewModels;
using PMF_Team01_MVC.Services;
using PMF_Team01_MVC.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace PMF_Team01_MVC.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _environment;
        
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger, ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment environment)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;

            _roleManager = roleManager;
            _environment = environment;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                if(_context.Users.Any(u=>u.Email == model.Email) == false)
                {
                    ModelState.AddModelError(string.Empty, "User doesn't exist.");
                    return View(model);
                }

                /*
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "User doesn't exist.");
                    return View(model);
                }
                */
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (!user.IsEnabled.HasValue)
                {

                    ModelState.AddModelError(string.Empty, "Admin approval pending.");
                    return View(model);

                } 
                else if (user.IsEnabled.Value == false)
                {
                    ModelState.AddModelError(string.Empty, "You have been banned!");
                    return View(model);
                }
                
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            List<SelectListItem> userRoles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Reader", Text = "Reader" },
                new SelectListItem { Value = "Author", Text = "Author" },
                new SelectListItem { Value = "Reviewer", Text = "Reviewer" }
            };

            //List<SelectListItem> cities = _context.Grad.Include(g=>g.Drzava).Select(g => new SelectListItem
            //{
            //    Value = g.Id.ToString(),
            //    Text = g.Naziv + " / " + g.Drzava.Naziv
            //}).ToList();

            RegisterViewModel model = new RegisterViewModel
            {
                Roles = userRoles,
                DateOfBirth = DateTime.Now
                //Cities = cities,

            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            List<SelectListItem> userRoles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Reader", Text = "Reader" },
                new SelectListItem { Value = "Author", Text = "Author" },
                new SelectListItem { Value = "Reviewer", Text = "Reviewer" }, 

            };

            model.Roles = userRoles;

            if (model.Role == "Reviewer" && model.CV == null)
            {
                ModelState.AddModelError(string.Empty, "Reviewers must upload CV!");
                
                return View(model);
            }
            
            if(model.Country == "selectCountry" || model.State == "selectState" || model.City == "selectCity")
            {
                ModelState.AddModelError(string.Empty, "Country, State and City are required!");
                return View(model);
            }

            //CREATING USER ROLES
            if (await _roleManager.RoleExistsAsync("Admin") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                });
            }

            if (await _roleManager.RoleExistsAsync("Reader") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Reader"
                });
            }

            if (await _roleManager.RoleExistsAsync("Author") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Author"
                });
            }

            if (await _roleManager.RoleExistsAsync("Reviewer") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Reviewer"
                });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Initials = model.Initials,
                    Affiliation = model.Affiliation,
                    DateOfBirth = model.DateOfBirth,
                    IsEnabled = null,
                    Country = model.Country,
                    State = model.State,
                    City = model.City
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                
                if (result.Succeeded)
                {
                    

                    if(model.Email == "matberin.spaha@hotmail.com")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }

                    switch (model.Role)
                    {
                        case "Reader":
                            await _userManager.AddToRoleAsync(user, "Reader");
                            break;
                        case "Author":
                            await _userManager.AddToRoleAsync(user, "Author");                            break;
                        case "Reviewer":
                            await _userManager.AddToRoleAsync(user, "Reviewer");
                            break;
                    }
                    
                    //svima se po defaultu
                    //dodaje uloga citaoca
                    if(model.Role != "Reader")
                    {
                        await _userManager.AddToRoleAsync(user, "Reader");
                    }

                    //CREATING USER DIRECTORY
                    string folderPath = Path.Combine(_environment.WebRootPath, 
                        "Uploads", user.UserName);

                    // if (!Directory.Exists(folderPath))

                    Directory.CreateDirectory(folderPath);
                    
                    //uploading user image
                    //kasnije omoguciti promjenu profilne slike

                    if(model.Image != null) { 
                    Image newImage = new Image
                    {
                        ApplicationUserId = user.Id,
                        FileName = model.Image.FileName
                    };

                    _context.Add(newImage);
                    _context.SaveChanges();

                    string imagePath = Path.Combine(folderPath, model.Image.FileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    }

                    //ADDING CV
                    //Doraditi
                    if(model.CV != null) { 
                        string CVPath = Path.Combine(folderPath, model.CV.FileName);
   
                        using (var stream = new FileStream(CVPath, FileMode.Create))
                        {
                            await model.CV.CopyToAsync(stream);
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> UpdateCV(IFormFile CV)
        {

            if(CV != null) {
                string ext = Path.GetExtension(CV.FileName);

                if (ext.Equals(".pdf")) { 

                var userId = _userManager.GetUserId(User);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            string folderPath = Path.Combine(_environment.WebRootPath,
                        "Uploads", user.UserName);

            //get all files from directory
            var dir = new DirectoryInfo(folderPath);

            var files = dir.GetFiles();

            //get .pdf file -> cv je jedini fajl sa nastavkom .pdf
            bool fileExists = files.Any(f => f.Name.Contains(".pdf"));

            if (fileExists)
            {
                var cvFile = files.FirstOrDefault(f => f.Name.Contains(".pdf"));
                cvFile.Delete();
            }

            //add new cv
            string newCVPath = Path.Combine(folderPath, CV.FileName);

            using (var stream = new FileStream(newCVPath, FileMode.Create))
            {
                await CV.CopyToAsync(stream);
            }
            }
            }

            return RedirectToAction("MyProfile");
        }

        public async Task<IActionResult> UpdateImage(IFormFile image)
        {
            if(image != null) {

                var ext = Path.GetExtension(image.FileName);

                if(ext.Equals(".jpg") || ext.Equals(".png")) { 
            var userId = _userManager.GetUserId(User);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            string folderPath = Path.Combine(_environment.WebRootPath,
                        "Uploads", user.UserName);

            var dbImage = _context.Image.FirstOrDefault(i => i.ApplicationUserId == user.Id);

            if (dbImage != null)
            {
                _context.Remove(dbImage);
            }

            
                Image newImage = new Image
                {
                    ApplicationUserId = user.Id,
                    FileName = image.FileName
                };

                _context.Add(newImage);

                _context.SaveChanges();

            
            
                string imagePath = Path.Combine(folderPath, image.FileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            }

            return RedirectToAction("MyProfile");
        }


        // [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult EditAccount(string id)
        {
            if (!_context.Users.Any(u => u.Id == id))
                return RedirectToAction("Home", "Index");

            var account = _context.Users
                .FirstOrDefault(u => u.Id == id);

            var model = new RegisterViewModel
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                DateOfBirth = account.DateOfBirth,
                Affiliation = account.Affiliation,
                //CityId = account.GradId.ToString(),
                Title = account.Title,
                Email = account.Email,
                Initials = account.Initials,
                City = account.City,
                Country = account.Country,
                State = account.State
               // Cities = cities
            };

            //foreach(var item in model.Cities)
            //{
            //    if(item.Value == model.CityId)
            //    {
            //        item.Selected = true;
            //        break;
            //    }
            //}

            return View(model);
        }

        [HttpPost]
        public IActionResult EditAccount(RegisterViewModel model)
        {

            if (model.Country == "selectCountry" || model.State == "selectState" || model.City == "selectCity")
            {
                ModelState.AddModelError(string.Empty, "Country, State and City are required!");
                return View(model);
            }

            if (String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName) ||
                String.IsNullOrEmpty(model.Title) || String.IsNullOrEmpty(model.Initials)
                || String.IsNullOrEmpty(model.Affiliation)){
                    return View(model);
            }

            //if (ModelState.IsValid) { 
                var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Title = model.Title;
                user.Initials = model.Initials;
                user.Affiliation = model.Affiliation;
                user.DateOfBirth = model.DateOfBirth;
            
            user.Country = model.Country;
            user.State = model.State;
            user.City = model.City;

            /*
            if (model.Image != null)
            {

            if(_context.Image.Any(i=>i.ApplicationUserId == user.Id)) {
                    var currentImage = _context.Image.FirstOrDefault(i => i.ApplicationUserId == user.Id);

                    string filePath = Path.Combine(_environment.WebRootPath,
                   "Uploads", user.UserName, currentImage.FileName);

                    var file = new FileInfo(filePath);

                    if (file.Exists)
                    {
                        file.Delete();

                        _context.Remove(currentImage);
                    }
            }

            Image newImage = new Image
            {
                ApplicationUserId = user.Id,
                FileName = model.Image.FileName
            };

            _context.Image.Add(newImage);

            string newFilePath = Path.Combine(_environment.WebRootPath, "Uploads", user.UserName);

            string newImagePath = Path.Combine(newFilePath, model.Image.FileName);

            using (var stream = new FileStream(newImagePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
        }
        */
            _context.SaveChanges();

                return RedirectToAction("MyProfile");
            //}
            //return View(model);
        }

        public IActionResult UpdatePassword(string id)
        {
            //???
            if(_context.Users.Any(u=>u.Id == id) == false)
            {
                return RedirectToAction("Home", "Index");
            }

            var account = _context.Users.FirstOrDefault(u => u.Id == id);

            var model = new UpdatePasswordVM
            {
                Id = account.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

            if(checkPasswordResult == false)
            {
                ModelState.AddModelError(string.Empty, "Incorrect current password!");
                return View(model);
            }

            var newPassword = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            user.PasswordHash = newPassword;

            var res = await _userManager.UpdateAsync(user);

            _context.SaveChanges();

            return RedirectToAction("EditAccount", "Account", new { @id = model.Id });
            //}
            //else
            //{
            //    ModelState.AddModelError("Error");
            //}
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);


            var roles = await _userManager.GetRolesAsync(user);

           // var grad = _context.Grad.FirstOrDefault(g => g.Id == user.GradId);

            //var drzava = _context.Drzava.FirstOrDefault(d => d.Id == grad.DrzavaId);

            var model = new MyProfileVM
            {
                Id = user.Id,
                Ime = user.FirstName,
                MiddleName = user.Middlename,
                Prezime = user.LastName,
                Email = user.Email,
                Affiliation = user.Affiliation,
                DatumRodjenja = user.DateOfBirth,
                Initials = user.Initials,
                Title = user.Title,
                Roles = roles.ToList(),
                Grad = user.City,
                Drzava = user.Country,
                State = user.State
                //,
                //Grad = grad.Naziv,
                //Drzava = drzava.Naziv
            };

            
            //if(_context.Image.Any(i=>i.ApplicationUserId == user.Id))
            //{ 

            var image = _context.Image.FirstOrDefault(u => u.ApplicationUserId == user.Id);

            if(image != null)
                model.ImagePath = "/Uploads/" + user.Email + "/" + image.FileName;
            //}

            string folderPath = Path.Combine(_environment.WebRootPath,
                        "Uploads", user.UserName);

            //get all files from directory
            var dir = new DirectoryInfo(folderPath);

            var files = dir.GetFiles();

            //get .pdf file -> cv je jedini fajl sa nastavkom .pdf
            //var cvFile = files.FirstOrDefault(f => f.Name.Contains(".pdf"));

            //if (cvFile.Exists)
            bool fileExists = files.Any(f => f.Name.Contains(".pdf"));

            if (fileExists) {
                var cvFile = files.FirstOrDefault(f => f.Name.Contains(".pdf"));
                model.CVPath = "/Uploads/" + user.UserName + "/" + cvFile.Name;
            }

            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        
        #endregion
    }
}
