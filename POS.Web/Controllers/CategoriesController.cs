using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Diagnostics;
using POS.Web.Models;
using POS.Business;
using POS.Entities;

using SQLitePCL;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace POS.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MySQLiteContext _context;
        private readonly BusinessCategory _manageCategory;

        public CategoriesController(MySQLiteContext context)
        {
            _context = context;
            _manageCategory = new BusinessCategory(_context);
        }

        // En el método Index, establece el valor predeterminado "DEFAULT" si User.Identity.Name es null
        public async Task<IActionResult> Index(string status = "AC")
        {
            

            IEnumerable<Category> categories = new List<Category>();

            try
            {
                categories = _manageCategory.GetAll(status);

                switch (status)
                {
                    case "AC":
                        ViewData["txtButton"] = "Dar de baja";
                        ViewData["txtTitle"] = "Activas";
                        ViewData["asp-route-status"] = "IN";
                        ViewData["asp-button"] = "Categorías Inactivas";
                        ViewData["option"] = "Inactivar";
                        break;
                    case "IN":
                        ViewData["txtButton"] = "Activar";
                        ViewData["txtTitle"] = "Inactivas";
                        ViewData["asp-route-status"] = "AC";
                        ViewData["asp-button"] = "Categorías Activas";
                        ViewData["option"] = "Activar";
                        break;
                    default:
                        ViewData["txtButton"] = "Dar de baja";
                        ViewData["txtTitle"] = "Activas";
                        ViewData["asp-route-status"] = "IN";
                        ViewData["asp-button"] = "Categorías Inactivas";
                        ViewData["option"] = "Inactivar";
                        break;
                }

                return View(categories);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                });
            }
        }

        // GET: Categorys/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Category category = new Category();

            try
            {                

                category = _manageCategory.GetById(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                });
            }

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                return View(category);
            }
        }

        // GET: Categorys/Create
        public IActionResult Create()
        {
            string username = User.Identity.Name ?? "DEFAULT";
            ViewData["User"] = username;
            return View();
        }

        // POST: Categorys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,Name,CreateUser,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _manageCategory.Add(category);

                    TempData["SuccessMessage"] = "Registro de categoría exitoso";

                    return RedirectToAction(nameof(Index), new { username = category.CreateUser });
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = ex.Message,
                        Source = ex.Source,
                        InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                        InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                    });
                }
            }
            else
            {
                ViewData["User"] = category.CreateUser;

                return View(category);
            }
        }

        // GET: Categorys/Edit/5
        public async Task<IActionResult> Edit(int id, string option)
        {
            Category category = new Category();            

            try
            {
                category = _manageCategory.GetById(id);

                if (category == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                });
            }

            switch (option)
            {
                case "Activar":
                    category.Status = "AC";
                    ViewData["txtbutton"] = "Activar";
                    ViewData["btn-class"] = "btn btn-success";
                    ViewData["txtTitle"] = "Activar";
                    break;
                case "Inactivar":
                    category.Status = "IN";
                    ViewData["txtbutton"] = "Dar de baja";
                    ViewData["btn-class"] = "btn btn-warning";
                    ViewData["txtTitle"] = "Inactivar";
                    break;
                default:
                    ViewData["txtbutton"] = "Actualizar";
                    ViewData["btn-class"] = "btn btn-primary";
                    ViewData["txtTitle"] = "Editar";
                    break;
            }

            string username = User.Identity.Name ?? "DEFAULT";

            ViewData["User"] = username;

            return View(category);
        }

        // POST: Categorys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategory,Name,Status," +
            "CreateUser,CreateDate,LastUpdateUser")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _manageCategory.Update(category);

                    TempData["SuccessMessage"] = "Actualización de categoría exitosa";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.IdCategory))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = ex.Message,
                        Source = ex.Source,
                        InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                        InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                    });
                }
            }
            else
            {
                ViewData["User"] = category.LastUpdateUser;

                return View(category);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Category category = new();

            try
            {
                category = _manageCategory.GetById(id);

                if (category == null)
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                });
            }

            return View(category);
        }

        // POST: Categorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _manageCategory.Delete(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.IdCategory == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
