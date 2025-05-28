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
using POS.Web.Models;

namespace POS.Web.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly MySQLiteContext _context;
        private readonly BusinessWarehouse _manageWarehouse;
        private readonly BusinessWarehouseLocation _manageWarehouseLocation;

        public WarehousesController(MySQLiteContext context)
        {
            _context = context;
            _manageWarehouse = new BusinessWarehouse(_context);
            _manageWarehouseLocation = new BusinessWarehouseLocation(_context);
        } 

        // GET: Warehouses
        public async Task<IActionResult> Index()
        {
            IEnumerable<Warehouse> warehouses = new List<Warehouse>();
            try
            {
                warehouses = _manageWarehouse.GetAll();

                return View(warehouses);

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

        // GET: Warehouses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Warehouse warehouse = new();

            try
            {
                warehouse = _manageWarehouse.GetById(id);
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

            if (warehouse == null)
            {
                return NotFound();
            }
            else
            {
                return View(warehouse);
            }
        }

        // GET: Warehouses/Create
        public IActionResult Create()
        {
            IEnumerable<WarehouseLocation> warehouseLocations = [];

            warehouseLocations = _manageWarehouseLocation.GetAll();

            ViewData["WarehouseLocation"] = warehouseLocations;

            return View();
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IdWL")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    warehouse.CreateUser = "Alta";

                    _manageWarehouse.Add(warehouse);

                    TempData["SuccessMessage"] = "Registro de almacén exitoso";

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
                IEnumerable<WarehouseLocation> warehouseLocations = [];

                warehouseLocations = _manageWarehouseLocation.GetAll();

                ViewData["WarehouseLocation"] = warehouseLocations;

                return View(warehouse);
            }
        }

        // GET: Warehouses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Warehouse warehouse = new();
            IEnumerable<WarehouseLocation> warehouseLocations = [];

            warehouseLocations = _manageWarehouseLocation.GetAll();

            ViewData["WarehouseLocation"] = warehouseLocations;

            warehouse = _manageWarehouse.GetById(id);  

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdWarehouse,Name,IdWL,CreateUser,CreateDate")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    warehouse.LastUpdateUser = "Editar";
                    warehouse.LastUpdateDate = DateTime.Now;

                    _manageWarehouse.Update(warehouse); 

                    TempData["SuccessMessage"] = "Actualización de datos exitosa";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.IdWarehouse))
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
                return View(warehouse);
            }
        }

        // GET: Warehouses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Warehouse warehouse = new();

            try
            {
                warehouse = _manageWarehouse.GetById(id);
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

            if (warehouse == null)
            {
                return NotFound();
            }
            else
            {
                return View(warehouse);
            }
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _manageWarehouse.Delete(id);
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

        private bool WarehouseExists(int id)
        {
            Warehouse warehouse = new();

            bool exist = false;

            try
            {
                warehouse = _manageWarehouse.GetById(id);
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
