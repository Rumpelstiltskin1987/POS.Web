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
using POS.Interfaces;

namespace POS.Controllers
{
    public class AccountController : Controller
    {
        private readonly MySQLiteContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(MySQLiteContext context, 
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
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

                var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {

                    //var user = await _userManager.FindByNameAsync(username);

                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    // El usuario ha sido bloqueado (si configuraste el bloqueo en IdentityOptions)
                    ModelState.AddModelError("", "La cuenta ha sido bloqueada debido a múltiples intentos fallidos.");
                    return View();
                }
                else if (result.IsNotAllowed)
                {
                    // El inicio de sesión no está permitido (ej. cuenta no confirmada, etc.)
                    ModelState.AddModelError("", "No se permite el inicio de sesión para este usuario.");
                    return View();
                }
                else // Esto cubre PasswordFailure y otras razones no específicas
                {
                    // Contraseña incorrecta o usuario no encontrado (para evitar enumeración de usuarios)
                    ModelState.AddModelError("", "Intento de inicio de sesión inválido.");
                    return View();
                }                
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
                //var existingUser = await _context.ApplicationUser
                //    .AnyAsync(u => u.UserName == model.UserName);

                var existingUser = await _userManager.FindByNameAsync(model.UserName);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "El nombre de usuario ya está en uso.");
                    return View(model);
                }

                // Hashear la contraseña antes de almacenarla
                //var passwordHasher = new PasswordHasher<ApplicationUser>();
                //model.PasswordHash = passwordHasher.HashPassword(model, model.PasswordHash);

                //_context.ApplicationUser.Add(model);
                //await _context.SaveChangesAsync();
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email, // Asegúrate de que tu ApplicationUser tenga una propiedad Email
                    FirstName = model.FirstName, // Si tienes estas propiedades personalizadas
                    LastName = model.LastName,   //
                    PasswordHash = model.PasswordHash // Otras propiedades que necesites
                };

                var result = await _userManager.CreateAsync(user, model.PasswordHash);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }

                // Si falla, añadir los errores de Identity a ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
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
