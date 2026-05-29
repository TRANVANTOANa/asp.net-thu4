using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Danh sách đơn hàng
        public IActionResult Index()
        {
            var orders = _context.Orders
                                 .Include(o => o.Customer)
                                 .Include(o => o.OrderDetails)
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToList();
            return View(orders);
        }

        // GET: Chi tiết đơn hàng
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.OrderDetails)
                                .ThenInclude(od => od.Product)
                                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Form thêm đơn hàng
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName");
            ViewBag.StatusList = GetStatusList();
            return View();
        }

        // POST: Lưu đơn hàng mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName");
                ViewBag.StatusList = GetStatusList();
                return View(model);
            }

            try
            {
                if (model.OrderDate == default)
                {
                    model.OrderDate = DateTime.Now;
                }

                _context.Orders.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi lưu đơn hàng: " + ex.Message);
                ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName");
                ViewBag.StatusList = GetStatusList();
                return View(model);
            }
        }

        // GET: Form sửa đơn hàng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName", order.CustomerId);
            ViewBag.StatusList = GetStatusList(order.Status);
            return View(order);
        }

        // POST: Lưu sửa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName", model.CustomerId);
                ViewBag.StatusList = GetStatusList(model.Status);
                return View(model);
            }

            try
            {
                var oldOrder = _context.Orders.AsNoTracking().FirstOrDefault(o => o.Id == model.Id);
                if (oldOrder == null) return NotFound();

                // Giữ lại ngày tạo ban đầu
                model.OrderDate = oldOrder.OrderDate;

                _context.Orders.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi cập nhật đơn hàng: " + ex.Message);
                ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName", model.CustomerId);
                ViewBag.StatusList = GetStatusList(model.Status);
                return View(model);
            }
        }

        // Xóa đơn hàng
        public IActionResult Delete(int id)
        {
            try
            {
                var order = _context.Orders.Find(id);
                if (order != null)
                {
                    // Xóa chi tiết đơn hàng trước
                    var orderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList();
                    _context.OrderDetails.RemoveRange(orderDetails);

                    // Sau đó xóa đơn hàng
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi xóa đơn hàng: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Helper method: Danh sách trạng thái
        private SelectList GetStatusList(int? selectedStatus = null)
        {
            var statuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Chờ duyệt" },
                new SelectListItem { Value = "1", Text = "Đang giao" },
                new SelectListItem { Value = "2", Text = "Đã xong" }
            };

            return new SelectList(statuses, "Value", "Text", selectedStatus);
        }
    }
}
