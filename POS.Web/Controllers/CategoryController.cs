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

namespace POS.Web.Controllers
{
    public class CategoryController : Controller
    {      
        private readonly MySQLiteContext _context;
        private readonly BusinessCategory _manageCategory;

        public CategoryController(MySQLiteContext context)
        {
            _context = context;
            _manageCategory = new BusinessCategory(_context);
        }

        // GET: Categorys
        public async Task<IActionResult> Index()
        {          
            try
            {
                IEnumerable<Category> categories = new List<Category>();

                categories = _manageCategory.GetAll();

                return View(categories);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
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
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
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
                        InnerExceptionMessage = ex.InnerException.Message,
                        InnerExceptionSource = ex.InnerException.Source
                    });
                }
            }
            return View(category);
        }

        // GET: Categorys/Edit/5
        public async Task<IActionResult> Edit(int id)
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
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
                });
            }

            
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
            string message = string.Empty;

            if (id != category.IdCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    message = _manageCategory.Update(category);
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
                        InnerExceptionMessage = ex.InnerException.Message,
                        InnerExceptionSource = ex.InnerException.Source
                    });
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categorys/Delete/5
        public async Task<IActionResult> Inactivate(int id)
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
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
                });
            }            

            return View(category);
        }

        // POST: Categorys/Delete/5
        [HttpPost, ActionName("Inactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InactivateConfirmed(int id, [Bind("IdCategory,Name,Status," +
            "CreateUser,CreateDate")] Category category)
        {
            string message = string.Empty;

            try
            {
                _manageCategory.Inactivate(category);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
                });
            }

            return RedirectToAction(nameof(Index));
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
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
                });
            }           

            return View(category);
        }

        // POST: Categorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string message = string.Empty;

            try
            {
                message = _manageCategory.Delete(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message,
                    Source = ex.Source,
                    InnerExceptionMessage = ex.InnerException.Message,
                    InnerExceptionSource = ex.InnerException.Source
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
