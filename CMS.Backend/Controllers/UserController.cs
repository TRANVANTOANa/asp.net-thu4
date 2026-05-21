using Microsoft.AspNetCore.Mvc;
using CMS.Data.entities; // Dùng lớp User và ApplicationDbContext
using System.Linq; // Cần thiết để sử dụng hàm .ToList()

namespace CMS.Backend.Controllers
{
    public class UserController : Controller
    {
        // 1. Khai báo thuộc tính ngữ cảnh cơ sở dữ liệu
        private readonly ApplicationDbContext _context;

        // 2. Sử dụng Constructor Injection để truyền DbContext vào Controller
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hàm Index: Hiển thị danh sách thành viên quản trị LẤY TỪ DATABASE THẬT
        public IActionResult Index()
        {
            // Lấy toàn bộ danh sách người dùng từ bảng User trong cơ sở dữ liệu
            var users = _context.Users.ToList();

            // Trả về View kèm theo danh sách người dùng thật
            return View(users);
        }
    }
}