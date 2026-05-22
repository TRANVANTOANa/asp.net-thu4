//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí đơn hàng
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
    // Lớp quản lý đơn hàng
    public class Order
    {
        // Mã định danh đơn hàng (khóa chính)
        [Key]
        public int Id { get; set; }

        // Ngày tạo đơn hàng, mặc định là ngày hiện tại
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Mã định danh khách hàng
        public int CustomerId { get; set; }

        // Trạng thái đơn hàng: 0 = Chờ duyệt, 1 = Đang giao, 2 = Đã xong
        public int Status { get; set; }

        // Ghi chú hoặc yêu cầu đặc biệt của khách hàng
        public string? Notes { get; set; }

        // Tham chiếu đến khách hàng
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        // Danh sách chi tiết các sản phẩm trong đơn hàng
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}

