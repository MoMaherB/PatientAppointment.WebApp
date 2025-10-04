using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
using PatientAppointment.WebApp.Models;
using System.Security.Claims;

namespace PatientAppointment.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Find the user in the database
                var user = _userRepository.GetByUsername(model.Username);
                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, user.Role)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "PatientAppointmentCookieAuth");
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync("PatientAppointmentCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                        return RedirectToAction("Index", "Home");

                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("PatientAppointmentCookieAuth");
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Admin")] 
        public IActionResult UserManagement()
        {
            var users = _userRepository.GetAll();

            return View(users);
        }

        [Authorize(Roles = "Admin")] 
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Extra security on the POST action
        public IActionResult CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userRepository.GetByUsername(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(model);
                }

                var passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(null, model.Password);

                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = hashedPassword,
                    Role = "Receptionist" 
                };

                _userRepository.Add(user);

                TempData["success"] = "User created successfully.";
                return RedirectToAction("UserManagement");
            }

            return View(model);
        }
        //public IActionResult GenerateHash()
        //{
        //    var passwordHasher = new PasswordHasher<User>();
        //    string passwordToHash = "Pa$$w0rd";

        //    // Hash the password
        //    string hashedPassword = passwordHasher.HashPassword(null, passwordToHash);

        //    // Return the hash as plain text so you can copy it
        //    return Content(hashedPassword);
        //}
    }
}
