//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí người dùng
//Ngày tạo:15/05/2026
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities
{
    // Lớp quản lý người dùng hệ thống
    public class User
    {
        // Mã định danh người dùng (khóa chính)
        public int Id { get; set; }

        // Tên đăng nhập của người dùng
        public string Username { get; set; }

        // Mật khẩu được mã hóa (hash)
        public string PasswordHash { get; set; }

        // Họ và tên của người dùng
        public string FullName { get; set; }

        // Vai trò của người dùng (Quản trị viên hoặc Biên tập viên)
        public string Role { get; set; }
    }
}
