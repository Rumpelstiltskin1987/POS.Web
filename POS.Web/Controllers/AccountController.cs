using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using POS.Entities;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using POS.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace POS.Controllers
{
    public class AccountController : Controller
    {
        private readonly MySQLiteContext _context;

        public AccountController(MySQLiteContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    ModelState.AddModelError("", "No existe el usuario " + username);
                    return View();
                }

                // Verificar la contraseña con el hasher
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);


                if (result != PasswordVerificationResult.Success)
                {
                    ModelState.AddModelError("", "Contraseña incorrecta.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("FullName", $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email ?? "")
                };

                var identity = new ClaimsIdentity(claims, "SmartStockAuth");                
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException?.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException?.Source ?? "No hay excepción interna"
                });
            }
        }


        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.ApplicationUser
                    .AnyAsync(u => u.UserName == model.UserName);

                if (existingUser)
                {
                    ModelState.AddModelError("", "El nombre de usuario ya está en uso.");
                    return View(model);
                }

                // Hashear la contraseña antes de almacenarla
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                model.PasswordHash = passwordHasher.HashPassword(model, model.PasswordHash);

                _context.ApplicationUser.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
