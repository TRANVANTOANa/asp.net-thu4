//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí sản phẩm
//Ngày tạo:15/05/2026
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Data.entities;

namespace CMS.Data.entities
{
    // Lớp quản lý sản phẩm
    public class Product
    {
        // Mã định danh sản phẩm (khóa chính)
        [Key]
        public int Id { get; set; }

        // Tên sản phẩm (bắt buộc)
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }

        // Mô tả chi tiết về sản phẩm
        public string? Description { get; set; }

        // Giá sản phẩm (phải lớn hơn hoặc bằng 0, theo định dạng decimal 18,2)
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Số lượng sản phẩm có sẵn trong kho
        public int StockQuantity { get; set; }

        // Đường dẫn hình ảnh của sản phẩm
        public string? ImageUrl { get; set; }

        // Mã định danh danh mục sản phẩm
        public int CategoryProductId { get; set; }

        // Tham chiếu đến danh mục sản phẩm
        [ForeignKey("CategoryProductId")]
        public virtual CategoryProduct? CategoryProduct { get; set; }
    }
}

