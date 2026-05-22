//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí danh mục
//Ngày tạo:15/05/2026

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities
{
    // Lớp quản lý danh mục bài viết
    public class Category
    {
        // Mã định danh danh mục (khóa chính)
        public int Id { get; set; }

        // Tên danh mục (ví dụ: Tin Giáo Dục)
        public string Name { get; set; }

        // Mô tả chi tiết về danh mục
        public string? Description { get; set; }

        // Danh sách các bài viết thuộc danh mục này (quan hệ một-nhiều)
        public virtual ICollection<Post> Posts { get; set; }
    }
}
