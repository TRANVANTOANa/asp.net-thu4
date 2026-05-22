//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí chi tiết đơn hàng
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
    // Lớp quản lý chi tiết đơn hàng
    public class OrderDetail
    {
        // Mã định danh chi tiết đơn hàng (khóa chính)
        [Key]
        public int Id { get; set; }

        // Mã định danh đơn hàng
        public int OrderId { get; set; }

        // Mã định danh sản phẩm
        public int ProductId { get; set; }

        // Số lượng sản phẩm được mua
        public int Quantity { get; set; }

        // Giá đơn vị tại thời điểm mua (theo định dạng decimal 18,2)
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Tham chiếu đến đơn hàng
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        // Tham chiếu đến sản phẩm
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
