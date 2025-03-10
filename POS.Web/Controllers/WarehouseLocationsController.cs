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
    public class WarehouseLocationsController : Controller
    {
        private readonly MySQLiteContext _context;

        public WarehouseLocationsController(MySQLiteContext context)
        {
            _context = context;
        }

        // GET: WarehouseLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.WarehouseLocation.ToListAsync());
        }

        // GET: WarehouseLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseLocation = await _context.WarehouseLocation
                .FirstOrDefaultAsync(m => m.IdWL == id);
            if (warehouseLocation == null)
            {
                return NotFound();
            }

            return View(warehouseLocation);
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
                _context.Add(warehouseLocation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registro de ubicación exitoso";
                return RedirectToAction(nameof(Index));
            }
            return View(warehouseLocation);
        }

        // GET: WarehouseLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseLocation = await _context.WarehouseLocation.FindAsync(id);
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
            if (id != warehouseLocation.IdWL)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouseLocation);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(warehouseLocation);
        }

        // GET: WarehouseLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouseLocation = await _context.WarehouseLocation
                .FirstOrDefaultAsync(m => m.IdWL == id);
            if (warehouseLocation == null)
            {
                return NotFound();
            }

            return View(warehouseLocation);
        }

        // POST: WarehouseLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouseLocation = await _context.WarehouseLocation.FindAsync(id);
            if (warehouseLocation != null)
            {
                _context.WarehouseLocation.Remove(warehouseLocation);
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
                Source = "Inactivar Ubicación",
                InnerExceptionMessage = "",
                InnerExceptionSource = ""
            });

        }

        private bool WarehouseLocationExists(int id)
        {
            return _context.WarehouseLocation.Any(e => e.IdWL == id);
        }
    }
}
