using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities
{
    // Lớp quản lý bài viết
    public class Post
    {
        // Mã định danh bài viết (khóa chính)
        public int Id { get; set; }

        // Tiêu đề bài viết
        public string Title { get; set; }

        // Nội dung chi tiết bài viết
        public string Content { get; set; }

        // Đường dẫn hình ảnh đại diện của bài viết
        public string ImageUrl { get; set; }

        // Ngày tạo bài viết, mặc định là ngày hiện tại
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Mã định danh danh mục bài viết
        public int CategoryId { get; set; }

        // Tham chiếu đến danh mục bài viết
        public virtual Category Category { get; set; }
    }
}
