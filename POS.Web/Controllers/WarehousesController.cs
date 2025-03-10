using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS.Entities;
using POS.Web.Models;

namespace POS.Web.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly MySQLiteContext _context;

        public WarehousesController(MySQLiteContext context)
        {
            _context = context;
        }

        // GET: Warehouses
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Warehouse.Include(x => x.WarehouseLocation).ToListAsync());
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

        // GET: Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                .Include(m => m.WarehouseLocation)
                .FirstOrDefaultAsync(m => m.IdWarehouse == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // GET: Warehouses/Create
        public IActionResult Create()
        {
            IEnumerable<WarehouseLocation> wl = new List<WarehouseLocation>();

            wl = _context.WarehouseLocation;

            var WarehouseLocation = wl.Select(c => new SelectListItem { Value = c.IdWL.ToString(), Text = c.Address })
                .ToList();

            ViewData["WarehouseLocation"] = WarehouseLocation;

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
                warehouse.CreateUser = "Alta";
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {            
            IEnumerable<WarehouseLocation> wl = new List<WarehouseLocation>();

            wl = _context.WarehouseLocation;

            var WarehouseLocation = wl.Select(c => new SelectListItem { Value = c.IdWL.ToString(), Text = c.Address })
                .ToList();

            ViewData["WarehouseLocations"] = WarehouseLocation;

            var warehouse = await _context.Warehouse.FindAsync(id);
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
            if (id != warehouse.IdWarehouse)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    warehouse.LastUpdateUser = "Editar";
                    warehouse.LastUpdateDate = DateTime.Now;

                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                .FirstOrDefaultAsync(m => m.IdWarehouse == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouse.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouse.Remove(warehouse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Inactivate(int? id)
        {

            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "Pagina en construcción",
                Source = "Inactivar Amacén",
                InnerExceptionMessage = "",
                InnerExceptionSource = ""
            });

        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouse.Any(e => e.IdWarehouse == id);
        }
    }
}
