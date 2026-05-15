using Microsoft.AspNetCore.Mvc;

using CMS.Data.entities;

namespace CMS.Backend.Controllers
{
    public class PostController : Controller
    {
        // Danh sách bài viết
        public IActionResult Index()
        {
            var posts = new List<Post>
            {
               new Post
    {
        Id = 1,
        Title = "Lộ trình học ASP.NET Core cho người mới",
        Content = "Nội dung bài viết về lộ trình học .NET...",
        ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3",
        CreatedDate = DateTime.Now
    },

    new Post
    {
        Id = 2,
        Title = "ReactJS và WebAPI: Xu hướng Fullstack 2026",
        Content = "Nội dung bài viết về sự kết hợp React và API...",
        ImageUrl = "https://images.unsplash.com/photo-1461749280684-dccba630e2f6",
        CreatedDate = DateTime.Now.AddDays(-1)
    },

    new Post
    {
        Id = 3,
        Title = "Hướng dẫn cài đặt môi trường Visual Studio",
        Content = "Các bước cài đặt công cụ cần thiết cho lập trình viên...",
        ImageUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085",
        CreatedDate = DateTime.Now.AddDays(-2)
    }
            };

            return View(posts);
        }

        // Chi tiết bài viết
        public IActionResult Details(int id)
        {
            var post = new Post
            {
                Id = id,
                Title = "Nội dung chi tiết bài viết số " + id,

                Content = "Đây là nội dung đầy đủ của bài viết mà bạn vừa click vào. " +
                          "Trang này dùng để hiển thị chi tiết bài viết trong ASP.NET Core MVC. " +
                          "Bạn có thể bổ sung thêm bình luận, tags, danh mục hoặc tác giả.",

                ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=1200&q=80",

                CreatedDate = DateTime.Now
            };

            return View(post);
        }
    }
}