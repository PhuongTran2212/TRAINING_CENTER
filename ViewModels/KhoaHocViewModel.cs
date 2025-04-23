using System;
using System.ComponentModel.DataAnnotations;

namespace TrungTamDaoTao.ViewModels
{
    public class KhoaHocViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên khóa học")]
        [Display(Name = "Tên khóa học")]
        public string TenKhoaHoc { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên giảng viên")]
        [Display(Name = "Giảng viên")]
        public string GiangVien { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn thời gian khai giảng")]
        [Display(Name = "Thời gian khai giảng")]
        [DataType(DataType.Date)]
        public DateTime ThoiGianKhaiGiang { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập học phí")]
        [Display(Name = "Học phí")]
        [Range(0, double.MaxValue, ErrorMessage = "Học phí không được âm")]
        public decimal HocPhi { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập số lượng học viên tối đa")]
        [Display(Name = "Số lượng học viên tối đa")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng học viên phải lớn hơn 0")]
        public int SoLuongToiDa { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }
    }
}