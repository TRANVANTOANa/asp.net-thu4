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

namespace CMS.Data.entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // Quản trị viên hoặc Biên tập viên
    }

}
