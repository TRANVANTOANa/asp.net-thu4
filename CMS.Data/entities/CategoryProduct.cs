//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí danh mục sản phẩm
//Ngày tạo:15/05/2026
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CMS.Data.entities
{
    // Lớp quản lý danh mục sản phẩm
    [Table("CategoriesProducts")]
    public class CategoryProduct
    {
        // Mã định danh danh mục sản phẩm (khóa chính)
        [Key]
        public int Id { get; set; }

        // Tên danh mục sản phẩm (bắt buộc, tối đa 100 ký tự)
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }

        // Mô tả chi tiết về danh mục sản phẩm
        public string? Description { get; set; }

        // Danh sách các sản phẩm thuộc danh mục này (quan hệ một-nhiều)
        public virtual ICollection<Product>? Products { get; set; }
    }
}

