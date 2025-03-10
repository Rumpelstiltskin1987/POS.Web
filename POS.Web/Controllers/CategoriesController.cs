using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: Categorys
        public async Task<IActionResult> Index(string status = "AC")
        {
            try
            {
                IEnumerable<Category> categories = new List<Category>();

                categories = _manageCategory.GetAll(status);

                switch (status)
                {
                    case "AC":
                        ViewData["txtButton"] = "Dar de baja";
                        ViewData["txtTitle"] = "Activas";
                        ViewData["asp-route"] = "IN";
                        ViewData["asp-button"] = "Categorías Inactivas";
                        ViewData["asp-action"] = "Edit";
                        ViewData["option"] = "Inactivar";
                        break;
                    case "IN":
                        ViewData["txtButton"] = "Activar";
                        ViewData["txtTitle"] = "Inactivas";
                        ViewData["asp-route"] = "AC";
                        ViewData["asp-button"] = "Categorías Activas";
                        ViewData["asp-action"] = "Edit";
                        ViewData["option"] = "Activar";
                        break;
                    default:
                        ViewData["txtButton"] = "Dar de baja";
                        ViewData["txtTitle"] = "Activas";
                        ViewData["asp-route"] = "IN";
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
            return View();
        }

        // POST: Categorys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _manageCategory.Add(category);

                    return RedirectToAction(nameof(Index));
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
            return View(category);
        }

        // GET: Categorys/Edit/5
        public async Task<IActionResult> Edit(int id, string option)
        {
            Category category = new Category();

            string txtbutton = string.Empty;

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
                    txtbutton = "Activar";
                    ViewData["btn-class"] = "btn btn-success";
                     break;
                case "Inactivar":
                    category.Status = "IN";
                    txtbutton = "Dar de baja";
                    ViewData["btn-class"] = "btn btn-warning";
                    break;
                default:
                    ViewData["btn-class"] = "btn btn-primary";
                    break;
            }

            ViewData["txtbutton"] = txtbutton;

            return View(category);
        }

        // POST: Categorys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategory,Name,Status," +
            "CreateUser,CreateDate")] Category category)
        {
            if (id != category.IdCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _manageCategory.Update(category);
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

                return RedirectToAction(nameof(Index));
            }
            return View(category);
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
    }
}
