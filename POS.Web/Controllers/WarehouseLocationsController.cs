using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS.Business;
using POS.Entities;
using POS.Interfaces;
using POS.Web.Models;

namespace POS.Web.Controllers
{
    public class WarehouseLocationsController : Controller
    {
        private readonly MySQLiteContext _context;
        private readonly BusinessWarehouseLocation _manageWarehouseLocation;

        public WarehouseLocationsController(MySQLiteContext context)
        {
            _context = context;
            _manageWarehouseLocation = new BusinessWarehouseLocation(_context);
        }

        // GET: WarehouseLocations
        public async Task<IActionResult> Index()
        {
            IEnumerable<WarehouseLocation> warehousLocations = new List<WarehouseLocation>();
            try
            {
                warehousLocations = _manageWarehouseLocation.GetAll();

                return View(warehousLocations);

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

        // GET: WarehouseLocations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            WarehouseLocation warehouseLocation = new();

            try
            {
                warehouseLocation = _manageWarehouseLocation.GetById(id);
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

            if (warehouseLocation == null)
            {
                return NotFound();
            }
            else
            {
                return View(warehouseLocation);
            }
        }

        // GET: WarehouseLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WarehouseLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdWL,Address")] WarehouseLocation warehouseLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    warehouseLocation.CreateUser = "Alta";

                    _manageWarehouseLocation.Add(warehouseLocation);

                    TempData["SuccessMessage"] = "Registro de dirección exitoso";

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
            else
            {
                return View(warehouseLocation);
            }
        }

        // GET: WarehouseLocations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            WarehouseLocation warehouseLocation = new();

            warehouseLocation = _manageWarehouseLocation.GetById(id);

            if (warehouseLocation == null)
            {
                return NotFound();
            }

            return View(warehouseLocation);
        }

        // POST: WarehouseLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdWL,Address")] WarehouseLocation warehouseLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    warehouseLocation.LastUpdateUser = "Editar";
                    warehouseLocation.LastUpdateDate = DateTime.Now;

                    _manageWarehouseLocation.Update(warehouseLocation);

                    TempData["SuccessMessage"] = "Actualización de datos exitosa";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseLocationExists(warehouseLocation.IdWL))
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
            return View(warehouseLocation);
        }

        // GET: WarehouseLocations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            WarehouseLocation warehouseLocation = new();

            try
            {
                warehouseLocation = _manageWarehouseLocation.GetById(id);
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

            if (warehouseLocation == null)
            {
                return NotFound();
            }
            else
            {
                return View(warehouseLocation);
            }
        }

        // POST: WarehouseLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _manageWarehouseLocation.Delete(id);
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

        private bool WarehouseLocationExists(int id)
        {
            WarehouseLocation warehouseLocation = new();

            bool exist = false;

            try
            {
                warehouseLocation = _manageWarehouseLocation.GetById(id);
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
