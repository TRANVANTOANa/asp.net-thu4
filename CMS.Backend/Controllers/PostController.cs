using Microsoft.AspNetCore.Mvc;
using CMS.Data.entities;
using System.Linq; // Thêm thư viện này để dùng các hàm LINQ (ToList, FirstOrDefault)

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        // 1. Khai báo biến ngữ cảnh cơ sở dữ liệu
        private readonly ApplicationDbContext _context;

        // 2. Inject ApplicationDbContext thông qua Constructor
        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách bài viết lấy từ DATABASE THẬT
        public IActionResult Index()
        {
            // Lấy toàn bộ danh sách bài viết trong bảng Post ra
            var posts = _context.Posts.ToList();

            return View(posts);
        }

        // Chi tiết bài viết lấy từ DATABASE THẬT theo Id
        public IActionResult Details(int id)
        {
            // Tìm bài viết có Id khớp với id truyền từ URL vào
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);

            // Nếu không tìm thấy bài viết nào, trả về trang lỗi 404 Not Found
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}