using System;
using System.ComponentModel.DataAnnotations;
using TrungTamDaoTao.Models;

namespace TrungTamDaoTao.ViewModels
{
    public class DangKyHocViewModel
    {
        public KhoaHoc KhoaHoc { get; set; }
        
        // Số lượng học viên đã đăng ký
        public int SoLuongDaDangKy { get; set; }
        
        // Số lượng chỗ còn lại
        public int SoLuongConLai { get; set; }
        
        // Đánh dấu học viên đã đăng ký khóa học này chưa
        public bool DaDangKy { get; set; }
        
        // Đánh dấu học viên có thể hủy đăng ký hay không (chưa khai giảng)
        public bool CoTheHuy { get; set; }
    }
}