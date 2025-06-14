﻿using System;
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
                        ViewData["txtButton"] = "Dar de baja";
                        ViewData["txtTitle"] = "Activos";
                        ViewData["asp-route-status"] = "IN";
                        ViewData["asp-button"] = "Productos Inactivos";
                        ViewData["option"] = "Inactivar";
                        break;
                    case "IN":
                        ViewData["txtButton"] = "Activar";
                        ViewData["txtTitle"] = "Inactivos";
                        ViewData["asp-route-status"] = "AC";
                        ViewData["asp-button"] = "Productos Activos";
                        ViewData["option"] = "Activar";
                        break;
                    default:
                        ViewData["txtButton"] = "Dar de baja";
                        ViewData["txtTitle"] = "Activas";
                        ViewData["asp-route-status"] = "IN";
                        ViewData["asp-button"] = "Productos Inactivos";
                        ViewData["option"] = "Inactivar";
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
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
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
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
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

            string username = User.Identity.Name ?? "DEFAULT";

            ViewData["Categories"] = categories;
            
            ViewData["User"] = username;

            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IdCategory,Price,MeasureUnit,UrlImage,CreateUser,Status")] Product product, IFormFile? ProductImage)
        {
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

                    return RedirectToAction(nameof(Index), new { username = product.CreateUser });
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
                var categories = _manageCategory.GetAll("AC")
                .Select(c => new SelectListItem { Value = c.IdCategory.ToString(), Text = c.Name })
                .ToList();

                ViewBag.Categories = categories;
                ViewData["User"] = product.CreateUser;  

                return View(product);
            }
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int id, string option)
        {
            Product product = new Product();

            try
            {
                string username = User.Identity.Name ?? "DEFAULT";

                product = _manageProduct.GetById(id);

                if (product == null)
                {
                    return NotFound();
                }

                var categories = _manageCategory.GetAll("AC")
                .Select(c => new SelectListItem { Value = c.IdCategory.ToString(), Text = c.Name })
                .ToList();                

                ViewBag.Categories = categories;
                ViewData["User"] = username;

                switch (option)
                {
                    case "Activar":
                        product.Status = "AC";
                        ViewData["txtbutton"] = "Activar";
                        ViewData["btn-class"] = "btn btn-success";
                        ViewData["txtTitle"] = "Activar";
                        break;
                    case "Inactivar":
                        product.Status = "IN";
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

                return View(product);
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

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduct,Name,Description,IdCategory,Price,Stock,MeasureUnit," +
            "UrlImage,Status,CreateUser,CreateDate")] Product product, IFormFile? ProductImage)
        {
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

                    TempData["SuccessMessage"] = "Actualización de categoría exitosa";

                    return RedirectToAction(nameof(Index));
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
                        InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                        InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
                    });
                }                
            }
            else
            {
                ViewData["User"] = product.LastUpdateUser;

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
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
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
                    InnerExceptionMessage = ex.InnerException.Message ?? "No hay excepción interna",
                    InnerExceptionSource = ex.InnerException.Source ?? "No hay excepción interna"
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
