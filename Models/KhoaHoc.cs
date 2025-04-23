using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace TrungTamDaoTao.Models
{
    public class KhoaHoc
    {
        public int KhoaHocId { get; set; }           // Mã khóa học
        public string TenKhoaHoc { get; set; }       // Tên khóa học
        public string GiangVien { get; set; }        // Giảng viên
        public DateTime ThoiGianKhaiGiang { get; set; } // Thời gian khai giảng
        public decimal HocPhi { get; set; }          // Học phí
        public int SoLuongToiDa { get; set; }        // Số lượng học viên tối đa

        public DateTime NgayBatDau { get; set; }     // Có thể bỏ nếu đã có ThoiGianKhaiGiang
        public DateTime NgayKetThuc { get; set; }

        public ICollection<DangKyHoc> DangKyHocs { get; set; }
    }
}
