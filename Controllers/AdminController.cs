// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using TrungTamDaoTao.Data;
// using TrungTamDaoTao.Models;

// namespace TrungTamDaoTao.Controllers
// {
//     [Authorize(Roles = "Admin")]
//     public class AdminController : Controller
//     {
//         private readonly AppDbContext _context;

//         public AdminController(AppDbContext context)
//         {
//             _context = context;
//         }

//         // GET: /Admin/Index
//         public async Task<IActionResult> Index()
//         {
//             // Tổng số học viên
//             int totalHocVien = await _context.HocViens.CountAsync();
            
//             // Tổng số khóa học
//             int totalKhoaHoc = await _context.KhoaHocs.CountAsync();
            
//             // Tổng số đăng ký trong tháng hiện tại
//             var currentMonth = DateTime.Now.Month;
//             var currentYear = DateTime.Now.Year;
//             int dangKyTrongThang = await _context.DangKyHocs
//                 .Where(d => d.NgayDangKy.Month == currentMonth && d.NgayDangKy.Year == currentYear)
//                 .CountAsync();
                
//             // Khóa học phổ biến (có nhiều đăng ký nhất)
//             var khoaHocPhoBien = await _context.KhoaHocs
//                 .OrderByDescending(k => k.DangKyHocs.Count)
//                 .Take(5)
//                 .ToListAsync();
                
//             // Đăng ký mới nhất
//             var dangKyMoiNhat = await _context.DangKyHocs
//                 .Include(d => d.HocVien)
//                 .Include(d => d.KhoaHoc)
//                 .OrderByDescending(d => d.NgayDangKy)
//                 .Take(10)
//                 .ToListAsync();

//             // Truyền dữ liệu thống kê qua ViewBag
//             ViewBag.TotalHocVien = totalHocVien;
//             ViewBag.TotalKhoaHoc = totalKhoaHoc;
//             ViewBag.DangKyTrongThang = dangKyTrongThang;
//             ViewBag.KhoaHocPhoBien = khoaHocPhoBien;
//             ViewBag.DangKyMoiNhat = dangKyMoiNhat;
            
//             return View();
//         }

//         // GET: /Admin/QuanLyNguoiDung
//         public async Task<IActionResult> QuanLyNguoiDung()
//         {
//             var hocViens = await _context.HocViens.ToListAsync();
//             return View(hocViens);
//         }

//         // GET: /Admin/ThongKe
//         public IActionResult ThongKe()
//         {
//             return View();
//         }

//         // GET: /Admin/CauHinh
//         public IActionResult CauHinh()
//         {
//             return View();
//         }
//     }
// }
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrungTamDaoTao.Data;
using TrungTamDaoTao.Models;
using TrungTamDaoTao.ViewModels;

namespace TrungTamDaoTao.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Index
        public async Task<IActionResult> Index()
        {
            // Tổng số học viên
            int totalHocVien = await _context.HocViens.CountAsync();
            
            // Tổng số khóa học
            int totalKhoaHoc = await _context.KhoaHocs.CountAsync();
            
            // Tổng số đăng ký trong tháng hiện tại
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            int dangKyTrongThang = await _context.DangKyHocs
                .Where(d => d.NgayDangKy.Month == currentMonth && d.NgayDangKy.Year == currentYear)
                .CountAsync();
                
            // Khóa học phổ biến (có nhiều đăng ký nhất)
            var khoaHocPhoBien = await _context.KhoaHocs
                .OrderByDescending(k => k.DangKyHocs.Count)
                .Take(5)
                .ToListAsync();
                
            // Đăng ký mới nhất
            var dangKyMoiNhat = await _context.DangKyHocs
                .Include(d => d.HocVien)
                .Include(d => d.KhoaHoc)
                .OrderByDescending(d => d.NgayDangKy)
                .Take(10)
                .ToListAsync();

            // Truyền dữ liệu thống kê qua ViewBag
            ViewBag.TotalHocVien = totalHocVien;
            ViewBag.TotalKhoaHoc = totalKhoaHoc;
            ViewBag.DangKyTrongThang = dangKyTrongThang;
            ViewBag.KhoaHocPhoBien = khoaHocPhoBien;
            ViewBag.DangKyMoiNhat = dangKyMoiNhat;
            
            return View();
        }

        // GET: /Admin/HocVien
        public async Task<IActionResult> DanhSachHocVien()
        {
            var danhSachHocVien = await _context.HocViens.ToListAsync();
            return View(danhSachHocVien);
        }

        // GET: /Admin/ThemHocVien
        public IActionResult CreateHocVien()
        {
            return View();
        }

        // POST: /Admin/ThemHocVien
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHocVien(DangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                if (await _context.HocViens.AnyAsync(h => h.TaiKhoan == model.TaiKhoan))
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản này đã được sử dụng");
                    return View(model);
                }

                // Kiểm tra xem email đã tồn tại chưa
                if (await _context.HocViens.AnyAsync(h => h.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng");
                    return View(model);
                }

                // Tạo học viên mới
                var hocVien = new HocVien
                {
                    HoTen = model.HoTen,
                    Email = model.Email,
                    NgaySinh = model.NgaySinh,
                    SoDienThoai = model.SoDienThoai,
                    TaiKhoan = model.TaiKhoan,
                    MatKhau = model.MatKhau // Trong ứng dụng thực tế, bạn nên mã hóa mật khẩu
                };

                _context.Add(hocVien);
                await _context.SaveChangesAsync();

                TempData["ThongBao"] = "Thêm học viên thành công!";
                return RedirectToAction("DanhSachHocVien");
            }

            return View(model);
        }
// GET: /Admin/DeleteHocVien/5
public async Task<IActionResult> DeleteHocVien(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var hocVien = await _context.HocViens
        .FirstOrDefaultAsync(m => m.HocVienId == id);
    if (hocVien == null)
    {
        return NotFound();
    }

    return View(hocVien);
}

// POST: /Admin/DeleteHocVien/5
[HttpPost, ActionName("DeleteHocVien")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteHocVienConfirmed(int id)
{
    var hocVien = await _context.HocViens.FindAsync(id);
    
    if (hocVien == null)
    {
        return NotFound();
    }
    
    // Kiểm tra xem học viên có đăng ký khóa học nào không
    var dangKyHoc = await _context.DangKyHocs
        .AnyAsync(d => d.HocVienId == id);
        
    if (dangKyHoc)
    {
        // Nếu có, xóa các đăng ký khóa học của học viên trước
        var dangKyList = await _context.DangKyHocs
            .Where(d => d.HocVienId == id)
            .ToListAsync();
            
        _context.DangKyHocs.RemoveRange(dangKyList);
    }
    
    // Sau đó xóa học viên
    _context.HocViens.Remove(hocVien);
    await _context.SaveChangesAsync();
    
    TempData["ThongBao"] = "Đã xóa học viên thành công!";
    return RedirectToAction(nameof(DanhSachHocVien));
}
// GET: /Admin/EditHocVien/5
public async Task<IActionResult> EditHocVien(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var hocVien = await _context.HocViens.FindAsync(id);
    if (hocVien == null)
    {
        return NotFound();
    }

    // Chuyển đổi thành ViewModel để sử dụng trong form
    var viewModel = new DangKyViewModel
    {
        HoTen = hocVien.HoTen,
        Email = hocVien.Email,
        NgaySinh = hocVien.NgaySinh,
        SoDienThoai = hocVien.SoDienThoai,
        TaiKhoan = hocVien.TaiKhoan,
        MatKhau = hocVien.MatKhau // Trong thực tế, bạn không nên gửi mật khẩu về view
    };

    return View(viewModel);
}

// POST: /Admin/EditHocVien/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditHocVien(int id, DangKyViewModel model)
{
    if (id == 0)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            var hocVien = await _context.HocViens.FindAsync(id);
            if (hocVien == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu tài khoản đã tồn tại (và không phải là tài khoản hiện tại)
            if (await _context.HocViens.AnyAsync(h => h.TaiKhoan == model.TaiKhoan && h.HocVienId != id))
            {
                ModelState.AddModelError("TaiKhoan", "Tài khoản này đã được sử dụng");
                return View(model);
            }

            // Kiểm tra nếu email đã tồn tại (và không phải là email hiện tại)
            if (await _context.HocViens.AnyAsync(h => h.Email == model.Email && h.HocVienId != id))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng");
                return View(model);
            }

            // Cập nhật thông tin học viên
            hocVien.HoTen = model.HoTen;
            hocVien.Email = model.Email;
            hocVien.NgaySinh = model.NgaySinh;
            hocVien.SoDienThoai = model.SoDienThoai;
            hocVien.TaiKhoan = model.TaiKhoan;

            // Chỉ cập nhật mật khẩu nếu đã nhập mật khẩu mới
            if (!string.IsNullOrEmpty(model.MatKhau))
            {
                hocVien.MatKhau = model.MatKhau; // Trong ứng dụng thực tế, bạn nên mã hóa mật khẩu
            }

            _context.Update(hocVien);
            await _context.SaveChangesAsync();
            
            TempData["ThongBao"] = "Cập nhật học viên thành công!";
            return RedirectToAction(nameof(DanhSachHocVien));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.HocViens.AnyAsync(h => h.HocVienId == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }
    return View(model);
}
// GET: /Admin/DetailsHocVien/5
public async Task<IActionResult> DetailsHocVien(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Fetch học viên with related registrations and courses
    var hocVien = await _context.HocViens
        .Include(h => h.DangKyHocs)
            .ThenInclude(d => d.KhoaHoc)
        .FirstOrDefaultAsync(m => m.HocVienId == id);

    if (hocVien == null)
    {
        return NotFound();
    }

    return View(hocVien);
}

        // GET: /Admin/CauHinh
        public IActionResult CauHinh()
        {
            return View();
        }
    }
}