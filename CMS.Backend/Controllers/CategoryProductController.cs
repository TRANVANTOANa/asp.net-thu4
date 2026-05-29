using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Backend.Controllers
{
    [Authorize]
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Danh sách danh mục sản phẩm
        public IActionResult Index()
        {
            var categoryProducts = _context.CategoryProducts.ToList();
            return View(categoryProducts);
        }

        // GET: Chi tiết danh mục sản phẩm
        public IActionResult Details(int id)
        {
            var categoryProduct = _context.CategoryProducts
                                         .Include(cp => cp.Products)
                                         .FirstOrDefault(cp => cp.Id == id);

            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // GET: Form thêm danh mục sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lưu danh mục sản phẩm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryProduct model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Kiểm tra tên danh mục trùng
                if (_context.CategoryProducts.Any(cp => cp.Name == model.Name))
                {
                    ModelState.AddModelError("Name", "Tên danh mục này đã tồn tại!");
                    return View(model);
                }

                _context.CategoryProducts.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi lưu danh mục: " + ex.Message);
                return View(model);
            }
        }

        // GET: Form sửa danh mục sản phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoryProduct = _context.CategoryProducts.Find(id);
            if (categoryProduct == null) return NotFound();

            return View(categoryProduct);
        }

        // POST: Lưu sửa danh mục sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryProduct model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Kiểm tra tên danh mục trùng (ngoại trừ bản ghi hiện tại)
                if (_context.CategoryProducts.Any(cp => cp.Name == model.Name && cp.Id != model.Id))
                {
                    ModelState.AddModelError("Name", "Tên danh mục này đã tồn tại!");
                    return View(model);
                }

                _context.CategoryProducts.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi cập nhật danh mục: " + ex.Message);
                return View(model);
            }
        }

        // Xóa danh mục sản phẩm
        public IActionResult Delete(int id)
        {
            try
            {
                var categoryProduct = _context.CategoryProducts.Find(id);
                if (categoryProduct != null)
                {
                    // Kiểm tra có sản phẩm liên quan
                    if (_context.Products.Any(p => p.CategoryProductId == id))
                    {
                        TempData["Error"] = "Không thể xóa danh mục có sản phẩm!";
                        return RedirectToAction("Index");
                    }

                    _context.CategoryProducts.Remove(categoryProduct);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi xóa danh mục: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
