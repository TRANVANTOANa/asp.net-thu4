using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.entities;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Danh sách sản phẩm
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.CategoryProduct).ToList();
            return View(products);
        }

        // GET: Chi tiết sản phẩm
        public IActionResult Details(int id)
        {
            var product = _context.Products
                                  .Include(p => p.CategoryProduct)
                                  .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Form thêm sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_context.CategoryProducts, "Id", "Name");
            return View();
        }

        // POST: Lưu sản phẩm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model, IFormFile uploadImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(_context.CategoryProducts, "Id", "Name");
                return View(model);
            }

            try
            {
                // Xử lý upload hình ảnh
                if (uploadImage != null && uploadImage.Length > 0)
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadImage.CopyTo(stream);
                    }

                    model.ImageUrl = "/uploads/" + fileName;
                }

                _context.Products.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi lưu sản phẩm: " + ex.Message);
                ViewBag.CategoryList = new SelectList(_context.CategoryProducts, "Id", "Name");
                return View(model);
            }
        }

        // GET: Form sửa sản phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.CategoryList = new SelectList(_context.CategoryProducts, "Id", "Name", product.CategoryProductId);
            return View(product);
        }

        // POST: Lưu sửa sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model, IFormFile uploadImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = new SelectList(_context.CategoryProducts, "Id", "Name", model.CategoryProductId);
                return View(model);
            }

            try
            {
                var oldProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == model.Id);
                if (oldProduct == null) return NotFound();

                // Xử lý upload hình ảnh
                if (uploadImage != null && uploadImage.Length > 0)
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadImage.CopyTo(stream);
                    }

                    model.ImageUrl = "/uploads/" + fileName;
                }
                else if (string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = oldProduct.ImageUrl;
                }

                _context.Products.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi cập nhật sản phẩm: " + ex.Message);
                ViewBag.CategoryList = new SelectList(_context.CategoryProducts, "Id", "Name", model.CategoryProductId);
                return View(model);
            }
        }

        // Xóa sản phẩm
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi xóa sản phẩm: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
