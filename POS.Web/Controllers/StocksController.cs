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
    public class StocksController : Controller
    {
        private readonly MySQLiteContext _context;

        public StocksController(MySQLiteContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            IEnumerable<Stock> stocks = new List<Stock>();

            stocks = _context.Stock
                .Include(x => x.Warehouse)
                .Include(x => x.Warehouse.WarehouseLocation)
                .Include(x => x.Product)
                .Include(x => x.Product.Category)
                .ToList();

            return View(stocks);
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.IdStock == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            IEnumerable<Warehouse> warehouses = new List<Warehouse>();
            IEnumerable<Product> products = new List<Product>();

            warehouses = _context.Warehouse.ToList();
            products = _context.Product.ToList();

            var Warehouse = warehouses.Select(c => new SelectListItem
            {
                Value = c.IdWarehouse.ToString(),
                Text = c.Name
            }).ToList();

            var Product = products.Select(c => new SelectListItem
            {
                Value = c.IdProduct.ToString(),
                Text = $"{c.Name} / {c.Description}"
            }).ToList();

            ViewData["Warehouse"] = Warehouse;
            ViewData["Product"] = Product;

            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdWarehouse,IdProduct,Quantity")] Stock stock)
        {
            if (ModelState.IsValid)
            {                
                stock.CreateUser = "Alta";

                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStock,IdWarehouse,IdProduct,IdCategory,Quantity,CreateUser,CreateDate,LastUpdateUser,LastUpdateDate")] Stock stock)
        {
            if (id != stock.IdStock)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.IdStock))
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
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .FirstOrDefaultAsync(m => m.IdStock == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            if (stock != null)
            {
                _context.Stock.Remove(stock);
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

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.IdStock == id);
        }
    }
}
