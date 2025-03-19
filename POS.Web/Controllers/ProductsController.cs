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
using Microsoft.CodeAnalysis.Options;
using POS.Interfaces;

namespace POS.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MySQLiteContext _context;
        private readonly BusinessProduct _manageProduct;
        private readonly BusinessCategory _manageCategory;

        public ProductsController(MySQLiteContext context)
        {
            _context = context;
            _manageProduct = new BusinessProduct(_context);
            _manageCategory = new BusinessCategory(_context); 
        }

        // GET: Productos
        public async Task<IActionResult> Index(string status = "AC")
        {
            IEnumerable<Product> products = new List<Product>();

            try
            {
                products = _manageProduct.GetAll(status);

                switch (status)
                {
                    case "AC":
                        ViewData["txtButton"] = "Inactivar";
                        ViewData["txtTitle"] = "Activos";
                        ViewData["asp-route"] = "IN";
                        ViewData["asp-button"] = "Productos Inactivos";
                        ViewData["asp-action"] = "Edit";
                        break;
                    case "IN":
                        ViewData["txtButton"] = "Activar";
                        ViewData["txtTitle"] = "Inactivos";
                        ViewData["asp-route"] = "AC";
                        ViewData["asp-button"] = "Productos Activos";
                        ViewData["asp-action"] = "Edit";
                        break;
                    default:
                        ViewData["txtButton"] = "Inactivar";
                        ViewData["txtTitle"] = "Activas";
                        ViewData["asp-route"] = "IN";
                        ViewData["asp-button"] = "Categorías Inactivas";
                        break;
                }

                return View(products);
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

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Product product = new();

            try
            {
                product = _manageProduct.GetById(id);
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

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return View(product);
            }
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            var categories = _manageCategory.GetAll("AC")
                .Select(c => new SelectListItem { Value = c.IdCategory.ToString(), Text = c.Name })
                .ToList();

            ViewData["Categories"] = categories; 

            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IdCategory,Price,MeasureUnit,UrlImage")] Product product, IFormFile? ProductImage)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    _manageProduct.Add(product);

                    if (ProductImage != null && ProductImage.Length > 0)
                    {
                        // Ruta donde se guardará la imagen
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", ProductImage.FileName);

                        // Guardar la imagen en la ruta especificada
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await ProductImage.CopyToAsync(stream);
                        }
                    }

                    TempData["SuccessMessage"] = "Registro de producto exitoso";

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
            else
            {
                var categories = _manageCategory.GetAll("AC")
                .Select(c => new SelectListItem { Value = c.IdCategory.ToString(), Text = c.Name })
                .ToList();

                ViewBag.Categories = categories;

                return View(product);
            }
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int id, string option)
        {
            Product product = new Product();

            string txtbutton = string.Empty;

            try
            {
                product = _manageProduct.GetById(id);
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

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                //var categories = _manageCategory.GetAll("AC")
                //    .Select(c => new SelectListItem { Value = c.IdCategory.ToString(), Text = c.Name })
                //    .ToList();

                //ViewData["Categories"] = categories;

                switch (option)
                {
                    case "Activar":
                        product.Status = "AC";
                        txtbutton = "Activar";
                        ViewData["btn-class"] = "btn btn-success";
                        break;
                    case "Inactivar":
                        product.Status = "IN";
                        txtbutton = "Dar de baja";
                        ViewData["btn-class"] = "btn btn-warning";
                        break;
                    default:
                        txtbutton = "Actualizar";
                        ViewData["btn-class"] = "btn btn-primary";
                        break;
                }

                ViewData["txtbutton"] = txtbutton;

                return View(product);
            }
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduct,Name,Description,IdCategory,Price,Stock,MeasureUnit," +
            "UrlImage,Status,CreateUser,CreateDate")] Product product, IFormFile? ProductImage)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    _manageProduct.Update(product);

                    if (ProductImage != null && ProductImage.Length > 0)
                    {
                        // Ruta donde se guardará la imagen
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", ProductImage.FileName);

                        // Guardar la imagen en la ruta especificada
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await ProductImage.CopyToAsync(stream);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(product.IdProduct))
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
            else
            {
                return View(product);
            }
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Product product = new();

            try
            {
                product = _manageProduct.GetById(id);
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

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return View(product);
            }
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string message = string.Empty;

            try
            {
                _manageProduct.Delete(id);
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

        private bool ProductsExists(int id)
        {
            Product product = new();

            bool exist = false;

            try
            {
                product = _manageProduct.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return exist;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
