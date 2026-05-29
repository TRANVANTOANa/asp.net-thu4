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
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Danh sách chi tiết đơn hàng
        public IActionResult Index(int? orderId)
        {
            IQueryable<OrderDetail> orderDetails = _context.OrderDetails
                                                          .Include(od => od.Order)
                                                          .Include(od => od.Product);

            if (orderId.HasValue)
            {
                orderDetails = orderDetails.Where(od => od.OrderId == orderId);
            }

            return View(orderDetails.ToList());
        }

        // GET: Form thêm chi tiết đơn hàng
        [HttpGet]
        public IActionResult Create(int? orderId)
        {
            ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");
            
            if (orderId.HasValue)
            {
                ViewBag.SelectedOrderId = orderId;
            }

            return View();
        }

        // POST: Lưu chi tiết đơn hàng mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderDetail model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                return View(model);
            }

            try
            {
                // Kiểm tra sản phẩm có đủ trong kho
                var product = _context.Products.Find(model.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Sản phẩm không tồn tại!");
                    ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                    ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                    return View(model);
                }

                if (product.StockQuantity < model.Quantity)
                {
                    ModelState.AddModelError("Quantity", $"Sản phẩm chỉ còn {product.StockQuantity} cái!");
                    ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                    ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                    return View(model);
                }

                // Kiểm tra nếu không nhập giá thì lấy giá từ sản phẩm
                if (model.UnitPrice == 0)
                {
                    model.UnitPrice = product.Price;
                }

                // Giảm số lượng sản phẩm trong kho
                product.StockQuantity -= model.Quantity;
                _context.Products.Update(product);

                _context.OrderDetails.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index", new { orderId = model.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi lưu chi tiết đơn hàng: " + ex.Message);
                ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                return View(model);
            }
        }

        // GET: Form sửa chi tiết đơn hàng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail == null) return NotFound();

            ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", orderDetail.OrderId);
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: Lưu sửa chi tiết đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderDetail model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                return View(model);
            }

            try
            {
                var oldOrderDetail = _context.OrderDetails.AsNoTracking().FirstOrDefault(od => od.Id == model.Id);
                if (oldOrderDetail == null) return NotFound();

                var product = _context.Products.Find(model.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Sản phẩm không tồn tại!");
                    ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                    ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                    return View(model);
                }

                // Điều chỉnh số lượng sản phẩm trong kho
                int quantityDifference = model.Quantity - oldOrderDetail.Quantity;
                if (product.StockQuantity < quantityDifference)
                {
                    ModelState.AddModelError("Quantity", $"Sản phẩm không đủ số lượng!");
                    ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                    ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                    return View(model);
                }

                product.StockQuantity -= quantityDifference;
                _context.Products.Update(product);

                _context.OrderDetails.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index", new { orderId = model.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi khi cập nhật chi tiết đơn hàng: " + ex.Message);
                ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", model.OrderId);
                ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", model.ProductId);
                return View(model);
            }
        }

        // Xóa chi tiết đơn hàng
        public IActionResult Delete(int id)
        {
            try
            {
                var orderDetail = _context.OrderDetails.Find(id);
                if (orderDetail != null)
                {
                    int orderId = orderDetail.OrderId;

                    // Trả lại số lượng sản phẩm vào kho
                    var product = _context.Products.Find(orderDetail.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += orderDetail.Quantity;
                        _context.Products.Update(product);
                    }

                    _context.OrderDetails.Remove(orderDetail);
                    _context.SaveChanges();

                    return RedirectToAction("Index", new { orderId = orderId });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi khi xóa chi tiết đơn hàng: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
