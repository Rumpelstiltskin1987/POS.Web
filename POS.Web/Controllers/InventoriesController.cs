using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS.Entities;
using POS.Interfaces;

namespace POS.Web.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly MySQLiteContext _context;

        public InventoriesController(MySQLiteContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventory
                .Include(c => c.Stock)
                .Include(c => c.Stock.Warehouse)    
                .Include(c => c.Stock.Product)
                .ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? idStock, int? idMovement)
        {
            if (idStock == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.IdStock == idStock && m.IdMovement == idMovement);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            var stock = _context.Stock.Include(s => s.Product).ToList();

            var stockList = stock
                .Where(c => c.Product != null) // Evita productos nulos
                .Select(c => new SelectListItem
                {
                    Value = c.IdStock.ToString(),
                    Text = $"{c.Product.Name} / {c.Product.Description}"
                })
                .ToList();

            var MovementType = new SelectList(new[]
            {
                new { Value = "EN", Text = "Entrada" },
                new { Value = "SA", Text = "Salida" },
                new { Value = "AJ", Text = "Ajuste" }
            }, "Value", "Text");

            ViewData["MovementType"] = MovementType;
            ViewData["Stock"] = stockList;

            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStock,MovementType,Quantity,Description")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                var stock = await _context.Stock.Where(c => c.IdStock == inventory.IdStock).FirstOrDefaultAsync();

                inventory.MovementUser = "Alta";

                inventory.IdMovement = (_context.Inventory
                    .Where(x => x.IdStock == inventory.IdStock)
                    .Max(x => (int?)x.IdMovement) ?? 0) + 1;

                switch (inventory.MovementType)
                {
                    case "EN":
                        stock.Quantity += inventory.Quantity;
                        break;
                    case "SA":
                        stock.Quantity -= inventory.Quantity;
                        break;
                    case "AJ":
                        stock.Quantity = inventory.Quantity;
                        break;
                    default:
                        break;
                }

                _context.Add(inventory);

                _context.Update(stock);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMovement,IdStock,MovementType,Quantity,Description,MovementUser,MovementDate")] Inventory inventory)
        {
            if (id != inventory.IdMovement)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.IdMovement))
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
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.IdMovement == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.IdMovement == id);
        }
    }
}
