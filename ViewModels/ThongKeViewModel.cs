using System;
using System.Collections.Generic;
using TrungTamDaoTao.Models;

namespace TrungTamDaoTao.ViewModels
{
    public class ThongKeViewModel
    {
        public List<ThongKeKhoaHocViewModel> ThongKeKhoaHocs { get; set; }
        public decimal TongDoanhThu { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }

    public class ThongKeKhoaHocViewModel
    {
        public KhoaHoc KhoaHoc { get; set; }
        public int SoLuongHocVien { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
