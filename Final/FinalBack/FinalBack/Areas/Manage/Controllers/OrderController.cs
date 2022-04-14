using FinalBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static FinalBack.Models.PaginatedList;

namespace FinalBack.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class OrderController : Controller
    {
        private readonly FinalContext _context;

        public OrderController(FinalContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var orders = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).AsQueryable();

            string pageSizeStr = _context.Settings.FirstOrDefault(x => x.Key == "PageSize").Value;
            int pageSize = pageSizeStr == null ? 3 : int.Parse(pageSizeStr);
            ; return View(PagenatedList<Order>.Create(orders, page, pageSize));
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Edit(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            if (order.Id == 0)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View();
            }

            Order existOrder = _context.Orders.FirstOrDefault(x => x.Id == order.Id);
            if (order == null) return NotFound();

            existOrder.Status = order.Status;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null) return NotFound();
            order.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }


        public IActionResult Restore(int id)
        {
            Order existorder = _context.Orders.FirstOrDefault(x => x.Id == id && x.IsDeleted);

            if (existorder == null)
                return NotFound();

            existorder.IsDeleted = false;
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
