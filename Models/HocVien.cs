using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TrungTamDaoTao.Data;

namespace TrungTamDaoTao.Models
{
    public class HocVien
    {
        public int HocVienId { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public DateTime NgaySinh { get; set; }

        public string SoDienThoai { get; set; }     // Thêm số điện thoại
        public string TaiKhoan { get; set; }        // Thêm tài khoản
        public string MatKhau { get; set; }         // Thêm mật khẩu

        public ICollection<DangKyHoc> DangKyHocs { get; set; }
    }
}
