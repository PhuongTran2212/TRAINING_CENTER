using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TrungTamDaoTao.Data;
using TrungTamDaoTao.Models;
using TrungTamDaoTao.ViewModels;

namespace TrungTamDaoTao.Controllers
{
    [Authorize] // Yêu cầu đăng nhập cho tất cả các action trong controller
    public class DangKyHocController : Controller
    {
        private readonly AppDbContext _context;

        public DangKyHocController(AppDbContext context)
        {
            _context = context;
        }

        // // GET: DangKyHoc/Index - Hiển thị danh sách khóa học đã đăng ký của học viên
        // public async Task<IActionResult> Index()
        // {
        //     // Lấy ID của học viên đang đăng nhập
        //     int hocVienId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //     // Lấy danh sách các khóa học đã đăng ký
        //     var dangKyHocs = await _context.DangKyHocs
        //         .Include(d => d.KhoaHoc)
        //         .Where(d => d.HocVienId == hocVienId)
        //         .OrderByDescending(d => d.NgayDangKy)
        //         .ToListAsync();

        //     return View(dangKyHocs);
        // }

        // // GET: DangKyHoc/DanhSachKhoaHoc - Hiển thị danh sách các khóa học có thể đăng ký
        // public async Task<IActionResult> DanhSachKhoaHoc()
        // {
        //     int hocVienId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //     // Lấy danh sách khóa học
        //     var khoaHocs = await _context.KhoaHocs
        //         .Include(k => k.DangKyHocs)
        //         .OrderBy(k => k.NgayBatDau)
        //         .ToListAsync();

        //     // Lấy danh sách ID khóa học mà học viên đã đăng ký
        //     var khoaHocDaDangKyIds = await _context.DangKyHocs
        //         .Where(d => d.HocVienId == hocVienId)
        //         .Select(d => d.KhoaHocId)
        //         .ToListAsync();

        //     // Tạo ViewModel cho mỗi khóa học
        //     var viewModels = khoaHocs.Select(k => new DangKyHocViewModel
        //     {
        //         KhoaHoc = k,
        //         SoLuongDaDangKy = k.DangKyHocs?.Count ?? 0,
        //         SoLuongConLai = k.SoLuongToiDa - (k.DangKyHocs?.Count ?? 0),
        //         DaDangKy = khoaHocDaDangKyIds.Contains(k.KhoaHocId),
        //         CoTheHuy = khoaHocDaDangKyIds.Contains(k.KhoaHocId) && DateTime.Now < k.ThoiGianKhaiGiang
        //     }).ToList();

        //     return View(viewModels);
        // }

        // // GET: DangKyHoc/ChiTiet/5 - Xem chi tiết khóa học trước khi đăng ký
        // public async Task<IActionResult> ChiTiet(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     int hocVienId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //     // Lấy thông tin khóa học
        //     var khoaHoc = await _context.KhoaHocs
        //         .Include(k => k.DangKyHocs)
        //         .FirstOrDefaultAsync(k => k.KhoaHocId == id);

        //     if (khoaHoc == null)
        //     {
        //         return NotFound();
        //     }

        //     // Kiểm tra xem học viên đã đăng ký chưa
        //     var daDangKy = await _context.DangKyHocs
        //         .AnyAsync(d => d.KhoaHocId == id && d.HocVienId == hocVienId);

        //     // Tạo ViewModel
        //     var viewModel = new DangKyHocViewModel
        //     {
        //         KhoaHoc = khoaHoc,
        //         SoLuongDaDangKy = khoaHoc.DangKyHocs?.Count ?? 0,
        //         SoLuongConLai = khoaHoc.SoLuongToiDa - (khoaHoc.DangKyHocs?.Count ?? 0),
        //         DaDangKy = daDangKy,
        //         CoTheHuy = daDangKy && DateTime.Now < khoaHoc.ThoiGianKhaiGiang
        //     };

        //     return View(viewModel);
        // }

        // // POST: DangKyHoc/DangKy/5 - Đăng ký khóa học
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DangKy(int id)
        // {
        //     int hocVienId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //     // Kiểm tra xem khóa học có tồn tại không
        //     var khoaHoc = await _context.KhoaHocs
        //         .Include(k => k.DangKyHocs)
        //         .FirstOrDefaultAsync(k => k.KhoaHocId == id);

        //     if (khoaHoc == null)
        //     {
        //         return NotFound();
        //     }

        //     // Kiểm tra xem học viên đã đăng ký khóa học này chưa
        //     var daDangKy = await _context.DangKyHocs
        //         .AnyAsync(d => d.KhoaHocId == id && d.HocVienId == hocVienId);

        //     if (daDangKy)
        //     {
        //         TempData["ErrorMessage"] = "Bạn đã đăng ký khóa học này rồi!";
        //         return RedirectToAction(nameof(ChiTiet), new { id });
        //     }

        //     // Kiểm tra số lượng đăng ký còn lại
        //     int soLuongDaDangKy = khoaHoc.DangKyHocs?.Count ?? 0;
        //     if (soLuongDaDangKy >= khoaHoc.SoLuongToiDa)
        //     {
        //         TempData["ErrorMessage"] = "Khóa học đã đầy, không thể đăng ký thêm!";
        //         return RedirectToAction(nameof(ChiTiet), new { id });
        //     }

        //     // Tiến hành đăng ký
        //     var dangKyHoc = new DangKyHoc
        //     {
        //         HocVienId = hocVienId,
        //         KhoaHocId = id,
        //         NgayDangKy = DateTime.Now
        //     };

        //     _context.Add(dangKyHoc);
        //     await _context.SaveChangesAsync();

        //     TempData["SuccessMessage"] = "Đăng ký khóa học thành công!";
        //     return RedirectToAction(nameof(Index));
        // }
public async Task<IActionResult> Index()
{
    // Lấy ID của học viên đang đăng nhập
    var hocVienIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(hocVienIdStr) || !int.TryParse(hocVienIdStr, out int hocVienId))
    {
        // Nếu không có ID hoặc không thể chuyển thành int, có thể trả về lỗi hoặc redirect
        return RedirectToAction("DangNhap", "Account");
    }

    // Lấy danh sách các khóa học đã đăng ký
    var dangKyHocs = await _context.DangKyHocs
        .Include(d => d.KhoaHoc)
        .Where(d => d.HocVienId == hocVienId)
        .OrderByDescending(d => d.NgayDangKy)
        .ToListAsync();

    return View(dangKyHocs);
}

public async Task<IActionResult> DanhSachKhoaHoc()
{
    var hocVienIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(hocVienIdStr) || !int.TryParse(hocVienIdStr, out int hocVienId))
    {
        return RedirectToAction("DangNhap", "Account");
    }

    // Lấy danh sách khóa học
    var khoaHocs = await _context.KhoaHocs
        .Include(k => k.DangKyHocs)
        .OrderBy(k => k.NgayBatDau)
        .ToListAsync();

    // Lấy danh sách ID khóa học mà học viên đã đăng ký
    var khoaHocDaDangKyIds = await _context.DangKyHocs
        .Where(d => d.HocVienId == hocVienId)
        .Select(d => d.KhoaHocId)
        .ToListAsync();

    // Tạo ViewModel cho mỗi khóa học
    var viewModels = khoaHocs.Select(k => new DangKyHocViewModel
    {
        KhoaHoc = k,
        SoLuongDaDangKy = k.DangKyHocs?.Count ?? 0,
        SoLuongConLai = k.SoLuongToiDa - (k.DangKyHocs?.Count ?? 0),
        DaDangKy = khoaHocDaDangKyIds.Contains(k.KhoaHocId),
        CoTheHuy = khoaHocDaDangKyIds.Contains(k.KhoaHocId) && DateTime.Now < k.ThoiGianKhaiGiang
    }).ToList();

    return View(viewModels);
}

public async Task<IActionResult> ChiTiet(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var hocVienIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(hocVienIdStr) || !int.TryParse(hocVienIdStr, out int hocVienId))
    {
        return RedirectToAction("DangNhap", "Account");
    }

    // Lấy thông tin khóa học
    var khoaHoc = await _context.KhoaHocs
        .Include(k => k.DangKyHocs)
        .FirstOrDefaultAsync(k => k.KhoaHocId == id);

    if (khoaHoc == null)
    {
        return NotFound();
    }

    // Kiểm tra xem học viên đã đăng ký chưa
    var daDangKy = await _context.DangKyHocs
        .AnyAsync(d => d.KhoaHocId == id && d.HocVienId == hocVienId);

    // Tạo ViewModel
    var viewModel = new DangKyHocViewModel
    {
        KhoaHoc = khoaHoc,
        SoLuongDaDangKy = khoaHoc.DangKyHocs?.Count ?? 0,
        SoLuongConLai = khoaHoc.SoLuongToiDa - (khoaHoc.DangKyHocs?.Count ?? 0),
        DaDangKy = daDangKy,
        CoTheHuy = daDangKy && DateTime.Now < khoaHoc.ThoiGianKhaiGiang
    };

    return View(viewModel);
}

public async Task<IActionResult> DangKy(int id)
{
    var hocVienIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(hocVienIdStr) || !int.TryParse(hocVienIdStr, out int hocVienId))
    {
        return RedirectToAction("DangNhap", "Account");
    }

    // Kiểm tra xem khóa học có tồn tại không
    var khoaHoc = await _context.KhoaHocs
        .Include(k => k.DangKyHocs)
        .FirstOrDefaultAsync(k => k.KhoaHocId == id);

    if (khoaHoc == null)
    {
        return NotFound();
    }

    // Kiểm tra xem học viên đã đăng ký khóa học này chưa
    var daDangKy = await _context.DangKyHocs
        .AnyAsync(d => d.KhoaHocId == id && d.HocVienId == hocVienId);

    if (daDangKy)
    {
        TempData["ErrorMessage"] = "Bạn đã đăng ký khóa học này rồi!";
        return RedirectToAction(nameof(ChiTiet), new { id });
    }

    // Kiểm tra số lượng đăng ký còn lại
    int soLuongDaDangKy = khoaHoc.DangKyHocs?.Count ?? 0;
    if (soLuongDaDangKy >= khoaHoc.SoLuongToiDa)
    {
        TempData["ErrorMessage"] = "Khóa học đã đầy, không thể đăng ký thêm!";
        return RedirectToAction(nameof(ChiTiet), new { id });
    }

    // Tiến hành đăng ký
    var dangKyHoc = new DangKyHoc
    {
        HocVienId = hocVienId,
        KhoaHocId = id,
        NgayDangKy = DateTime.Now
    };

    _context.Add(dangKyHoc);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Đăng ký khóa học thành công!";
    return RedirectToAction(nameof(Index));
}

        // POST: DangKyHoc/HuyDangKy/5 - Hủy đăng ký khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDangKy(int id)
        {
            int hocVienId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Kiểm tra xem khóa học có tồn tại không
            var khoaHoc = await _context.KhoaHocs
                .FirstOrDefaultAsync(k => k.KhoaHocId == id);

            if (khoaHoc == null)
            {
                return NotFound();
            }

            // Kiểm tra xem học viên có đăng ký khóa học này không
            var dangKyHoc = await _context.DangKyHocs
                .FirstOrDefaultAsync(d => d.KhoaHocId == id && d.HocVienId == hocVienId);

            if (dangKyHoc == null)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng ký khóa học này!";
                return RedirectToAction(nameof(ChiTiet), new { id });
            }

            // Kiểm tra thời gian hủy đăng ký
            if (DateTime.Now >= khoaHoc.ThoiGianKhaiGiang)
            {
                TempData["ErrorMessage"] = "Khóa học đã khai giảng, không thể hủy đăng ký!";
                return RedirectToAction(nameof(ChiTiet), new { id });
            }

            // Tiến hành hủy đăng ký
            _context.DangKyHocs.Remove(dangKyHoc);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Hủy đăng ký khóa học thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}