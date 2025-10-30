using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrungTamDaoTao.Data;
using TrungTamDaoTao.ViewModels;

namespace TrungTamDaoTao.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ThongKeController : Controller
    {
        private readonly AppDbContext _context;

        public ThongKeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ThongKe
        public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay)
        {
            // Nếu không có ngày được chọn, mặc định lấy thống kê tháng hiện tại
            if (!tuNgay.HasValue)
            {
                tuNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (!denNgay.HasValue)
            {
                denNgay = DateTime.Now;
            }

            var khoaHocs = await _context.KhoaHocs
                .Include(k => k.DangKyHocs)
                .ThenInclude(d => d.HocVien)
                .ToListAsync();

            var thongKeViewModel = new ThongKeViewModel
            {
                ThongKeKhoaHocs = new List<ThongKeKhoaHocViewModel>(),
                TuNgay = tuNgay,
                DenNgay = denNgay
            };

            decimal tongDoanhThu = 0;

            foreach (var khoaHoc in khoaHocs)
            {
                // Lọc đăng ký học theo khoảng thời gian
                var dangKyHocs = khoaHoc.DangKyHocs
                    .Where(d => d.NgayDangKy >= tuNgay && d.NgayDangKy <= denNgay)
                    .ToList();

                int soLuongHocVien = dangKyHocs.Count;
                decimal doanhThu = soLuongHocVien * khoaHoc.HocPhi;
                tongDoanhThu += doanhThu;

                thongKeViewModel.ThongKeKhoaHocs.Add(new ThongKeKhoaHocViewModel
                {
                    KhoaHoc = khoaHoc,
                    SoLuongHocVien = soLuongHocVien,
                    DoanhThu = doanhThu
                });
            }

            thongKeViewModel.TongDoanhThu = tongDoanhThu;

            return View(thongKeViewModel);
        }

        // GET: ThongKe/DoanhThuTheoThang
        public async Task<IActionResult> DoanhThuTheoThang(int nam = 0)
        {
            if (nam == 0)
            {
                nam = DateTime.Now.Year;
            }

            var doanhThuTheoThang = new decimal[12];
            
            // Lấy tất cả đăng ký học trong năm được chọn
            var dangKyHocsNam = await _context.DangKyHocs
                .Include(d => d.KhoaHoc)
                .Where(d => d.NgayDangKy.Year == nam)
                .ToListAsync();

            // Tính doanh thu theo từng tháng
            for (int thang = 1; thang <= 12; thang++)
            {
                var dangKyHocsThang = dangKyHocsNam
                    .Where(d => d.NgayDangKy.Month == thang)
                    .ToList();

                decimal doanhThuThang = 0;
                foreach (var dangKy in dangKyHocsThang)
                {
                    doanhThuThang += dangKy.KhoaHoc.HocPhi;
                }

                doanhThuTheoThang[thang - 1] = doanhThuThang;
            }

            ViewBag.Nam = nam;
            ViewBag.DoanhThuTheoThang = doanhThuTheoThang;
            
            return View();
        }

        // GET: ThongKe/SoLuongHocVien
        public async Task<IActionResult> SoLuongHocVien()
        {
            var khoaHocs = await _context.KhoaHocs
                .Include(k => k.DangKyHocs)
                .ToListAsync();

            var tenKhoaHocs = khoaHocs.Select(k => k.TenKhoaHoc).ToList();
            var soLuongHocViens = khoaHocs.Select(k => k.DangKyHocs.Count).ToList();

            ViewBag.TenKhoaHocs = tenKhoaHocs;
            ViewBag.SoLuongHocViens = soLuongHocViens;

            return View();
        }
    }
}
