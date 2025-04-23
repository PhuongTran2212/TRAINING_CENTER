using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TrungTamDaoTao.Data;
using TrungTamDaoTao.Models;
using TrungTamDaoTao.ViewModels;
using Microsoft.AspNetCore.Authorization; //batdangnhap


namespace TrungTamDaoTao.Controllers
{
    public class KhoaHocController : Controller
    {
        private readonly AppDbContext _context;

        public KhoaHocController(AppDbContext context)
        {
            _context = context;
        }

        // GET: KhoaHoc
        public async Task<IActionResult> Index()
        {
            var khoaHocs = await _context.KhoaHocs.ToListAsync();
            return View(khoaHocs);
        }

        // GET: KhoaHoc/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhoaHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(KhoaHocViewModel model)
        {
            if (ModelState.IsValid)
            {
                var khoaHoc = new KhoaHoc
                {
                    TenKhoaHoc = model.TenKhoaHoc,
                    GiangVien = model.GiangVien,
                    ThoiGianKhaiGiang = model.ThoiGianKhaiGiang,
                    HocPhi = model.HocPhi,
                    SoLuongToiDa = model.SoLuongToiDa,
                    NgayBatDau = model.NgayBatDau,
                    NgayKetThuc = model.NgayKetThuc
                };

                _context.Add(khoaHoc);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo khóa học thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
// GET: KhoaHoc/Delete/5
[Authorize]
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var khoaHoc = await _context.KhoaHocs
        .Include(k => k.DangKyHocs)
        .FirstOrDefaultAsync(m => m.KhoaHocId == id);
        
    if (khoaHoc == null)
    {
        return NotFound();
    }

    // Kiểm tra xem có học viên nào đã đăng ký không
    ViewBag.SoLuongDangKy = khoaHoc.DangKyHocs?.Count ?? 0;

    return View(khoaHoc);
}

// POST: KhoaHoc/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
[Authorize]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var khoaHoc = await _context.KhoaHocs
        .Include(k => k.DangKyHocs)
        .FirstOrDefaultAsync(m => m.KhoaHocId == id);

    if (khoaHoc == null)
    {
        return NotFound();
    }

    // Kiểm tra xem khóa học có học viên đăng ký không
    if (khoaHoc.DangKyHocs != null && khoaHoc.DangKyHocs.Count > 0)
    {
        // Có học viên đăng ký, xóa tất cả các đăng ký trước
        _context.DangKyHocs.RemoveRange(khoaHoc.DangKyHocs);
    }

    // Sau đó xóa khóa học
    _context.KhoaHocs.Remove(khoaHoc);
    await _context.SaveChangesAsync();
    
    TempData["SuccessMessage"] = "Xóa khóa học thành công!";
    return RedirectToAction(nameof(Index));
}
// GET: KhoaHoc/Details/5
[Authorize]
public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var khoaHoc = await _context.KhoaHocs
        .Include(k => k.DangKyHocs)
            .ThenInclude(dk => dk.HocVien)
        .FirstOrDefaultAsync(m => m.KhoaHocId == id);
        
    if (khoaHoc == null)
    {
        return NotFound();
    }

    // Tính số lượng học viên đã đăng ký
    ViewBag.SoLuongDangKy = khoaHoc.DangKyHocs?.Count ?? 0;
    
    // Tính số lượng chỗ còn trống
    ViewBag.SoLuongConTrong = khoaHoc.SoLuongToiDa - (khoaHoc.DangKyHocs?.Count ?? 0);

    return View(khoaHoc);
}
// GET: KhoaHoc/Edit/5
[Authorize]
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var khoaHoc = await _context.KhoaHocs.FindAsync(id);
    if (khoaHoc == null)
    {
        return NotFound();
    }

    // Convertir en ViewModel pour l'édition
    var viewModel = new KhoaHocViewModel
    {
        TenKhoaHoc = khoaHoc.TenKhoaHoc,
        GiangVien = khoaHoc.GiangVien,
        ThoiGianKhaiGiang = khoaHoc.ThoiGianKhaiGiang,
        HocPhi = khoaHoc.HocPhi,
        SoLuongToiDa = khoaHoc.SoLuongToiDa,
        NgayBatDau = khoaHoc.NgayBatDau,
        NgayKetThuc = khoaHoc.NgayKetThuc
    };

    ViewBag.KhoaHocId = id;
    return View(viewModel);
}

// POST: KhoaHoc/Edit/5
[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, KhoaHocViewModel model)
{
    if (!ModelState.IsValid)
    {
        ViewBag.KhoaHocId = id;
        return View(model);
    }

    try
    {
        // Récupérer l'entité existante
        var khoaHoc = await _context.KhoaHocs.FindAsync(id);
        if (khoaHoc == null)
        {
            return NotFound();
        }

        // Mettre à jour les propriétés
        khoaHoc.TenKhoaHoc = model.TenKhoaHoc;
        khoaHoc.GiangVien = model.GiangVien;
        khoaHoc.ThoiGianKhaiGiang = model.ThoiGianKhaiGiang;
        khoaHoc.HocPhi = model.HocPhi;
        khoaHoc.SoLuongToiDa = model.SoLuongToiDa;
        khoaHoc.NgayBatDau = model.NgayBatDau;
        khoaHoc.NgayKetThuc = model.NgayKetThuc;

        // Vérifier si la capacité maximale est suffisante par rapport aux inscriptions existantes
        int nombreInscriptions = await _context.DangKyHocs.CountAsync(dk => dk.KhoaHocId == id);
        if (model.SoLuongToiDa < nombreInscriptions)
        {
            ModelState.AddModelError("SoLuongToiDa", $"Le nombre maximum d'étudiants ne peut pas être inférieur au nombre d'inscrits actuel ({nombreInscriptions})");
            ViewBag.KhoaHocId = id;
            return View(model);
        }

        _context.Update(khoaHoc);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Mise à jour du cours réussie !";
        return RedirectToAction(nameof(Index));
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.KhoaHocs.Any(e => e.KhoaHocId == id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }
}
    }
}