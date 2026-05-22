//Sinh viên:Trần Văn Toàn
//Mssv:2123110187
//lớp:CCQ2311F
//Mô tả:quản lí khách hàng
//Ngày tạo:15/05/2026
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace CMS.Data.entities
{
    // Lớp quản lý khách hàng
    public class Customer
    {
        // Mã định danh khách hàng (khóa chính)
        [Key]
        public int Id { get; set; }

        // Họ và tên khách hàng (bắt buộc)
        [Required]
        public string FullName { get; set; }

        // Địa chỉ email khách hàng (bắt buộc, phải là email hợp lệ)
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Số điện thoại khách hàng
        public string? Phone { get; set; }

        // Địa chỉ khách hàng
        public string? Address { get; set; }

        // Mật khẩu khách hàng (bắt buộc, lưu dưới dạng thô theo yêu cầu tối giản)
        [Required]
        public string Password { get; set; }

        // Danh sách các đơn hàng của khách hàng
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
