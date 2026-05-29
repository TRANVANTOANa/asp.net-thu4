using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Danh sách khách hàng
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        // GET: Chi tiết khách hàng
        public IActionResult Details(int id)
        {
            var customer = _context.Customers
                                   .Include(c => c.Orders)
                                   .FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Form thêm khách hàng
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lưu khách hàng mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Kiểm tra email đã tồn tại
                if (_context.Customers.Any(c => c.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký!");
                    return View(model);
                }

                // Kiểm tra số điện thoại nếu có
                if (!string.IsNullOrEmpty(model.Phone) && _context.Customers.Any(c => c.Phone == model.Phone))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại này đã tồn tại!");
                    return View(model);
                }

                _context.Customers.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi lưu khách hàng: " + ex.Message);
                return View(model);
            }
        }

        // GET: Form sửa khách hàng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Lưu sửa khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var oldCustomer = _context.Customers.AsNoTracking().FirstOrDefault(c => c.Id == model.Id);
                if (oldCustomer == null) return NotFound();

                // Kiểm tra email trùng (ngoại trừ bản ghi hiện tại)
                if (_context.Customers.Any(c => c.Email == model.Email && c.Id != model.Id))
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký!");
                    return View(model);
                }

                // Kiểm tra số điện thoại trùng (ngoại trừ bản ghi hiện tại)
                if (!string.IsNullOrEmpty(model.Phone) && 
                    _context.Customers.Any(c => c.Phone == model.Phone && c.Id != model.Id))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại này đã tồn tại!");
                    return View(model);
                }

                _context.Customers.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi cập nhật khách hàng: " + ex.Message);
                return View(model);
            }
        }

        // Xóa khách hàng
        public IActionResult Delete(int id)
        {
            try
            {
                var customer = _context.Customers.Find(id);
                if (customer != null)
                {
                    // Kiểm tra có đơn hàng liên quan
                    if (_context.Orders.Any(o => o.CustomerId == id))
                    {
                        TempData["Error"] = "Không thể xóa khách hàng có đơn hàng!";
                        return RedirectToAction("Index");
                    }

                    _context.Customers.Remove(customer);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi xóa khách hàng: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
