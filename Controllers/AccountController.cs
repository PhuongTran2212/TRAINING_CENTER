using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TrungTamDaoTao.Data;
using TrungTamDaoTao.Models;
using TrungTamDaoTao.ViewModels;

namespace TrungTamDaoTao.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: /Account/DangNhap
        public IActionResult DangNhap(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
// POST: /Account/DangNhap
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DangNhap(DangNhapViewModel model, string returnUrl = null)
{
    ViewData["ReturnUrl"] = returnUrl;

    if (ModelState.IsValid)
    {
        // --- 1. Kiểm tra tài khoản admin từ appsettings.json ---
        var adminTaiKhoan = _configuration["AdminAccount:TaiKhoan"];
        var adminMatKhau = _configuration["AdminAccount:MatKhau"];
        var adminHoTen = _configuration["AdminAccount:HoTen"];

        if (model.TaiKhoan == adminTaiKhoan && model.MatKhau == adminMatKhau)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, adminHoTen),
                new Claim(ClaimTypes.NameIdentifier, "admin"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe, // Sử dụng RememberMe để xác định phiên đăng nhập có lưu lâu dài hay không
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
                });

            // Chuyển hướng đến trang Admin Dashboard
            return RedirectToAction("Index", "Admin");
        }

        // --- 2. Kiểm tra tài khoản học viên trong database ---
        var hocVien = await _context.HocViens
            .FirstOrDefaultAsync(h => h.TaiKhoan == model.TaiKhoan);

        if (hocVien != null && hocVien.MatKhau == model.MatKhau)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, hocVien.HoTen),
                new Claim(ClaimTypes.NameIdentifier, hocVien.HocVienId.ToString()),
                new Claim(ClaimTypes.Role, "HocVien")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
                });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng");
    }

    return View(model);
}

        // GET: /Account/DangKy
        public IActionResult DangKy()
        {
            return View();
        }

        // POST: /Account/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(DangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                if (await _context.HocViens.AnyAsync(h => h.TaiKhoan == model.TaiKhoan))
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản này đã được sử dụng");
                    return View(model);
                }

                if (await _context.HocViens.AnyAsync(h => h.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng");
                    return View(model);
                }

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

                // Đăng nhập tự động sau khi đăng ký
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, hocVien.HoTen),
                    new Claim(ClaimTypes.NameIdentifier, hocVien.HocVienId.ToString()),
                    new Claim(ClaimTypes.Role, "HocVien")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // POST: /Account/DangXuat
        [HttpPost]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
// [HttpPost]
// [ValidateAntiForgeryToken]
// public async Task<IActionResult> DangNhap(DangNhapViewModel model, string returnUrl = null)
// {
//     ViewData["ReturnUrl"] = returnUrl;

//     if (ModelState.IsValid)
//     {
//         // --- 1. Kiểm tra tài khoản admin từ appsettings.json ---
//         var adminTaiKhoan = _configuration["AdminAccount:TaiKhoan"];
//         var adminMatKhau = _configuration["AdminAccount:MatKhau"];
//         var adminHoTen = _configuration["AdminAccount:HoTen"];

//         if (model.TaiKhoan == adminTaiKhoan && model.MatKhau == adminMatKhau)
//         {
//             var claims = new List<Claim>
//             {
//                 new Claim(ClaimTypes.Name, adminHoTen),
//                 new Claim(ClaimTypes.NameIdentifier, "admin"),
//                 new Claim(ClaimTypes.Role, "Admin")
//             };

//             var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//             await HttpContext.SignInAsync(
//                 CookieAuthenticationDefaults.AuthenticationScheme,
//                 new ClaimsPrincipal(claimsIdentity));

//             return RedirectToAction("Index", "Admin"); // Chuyển tới trang admin
//         }

//         // --- 2. Kiểm tra tài khoản học viên trong database ---
//         var hocVien = await _context.HocViens
//             .FirstOrDefaultAsync(h => h.TaiKhoan == model.TaiKhoan);

//         if (hocVien != null && hocVien.MatKhau == model.MatKhau)
//         {
//             var claims = new List<Claim>
//             {
//                 new Claim(ClaimTypes.Name, hocVien.HoTen),
//                 new Claim(ClaimTypes.NameIdentifier, hocVien.HocVienId.ToString()),
//                 new Claim(ClaimTypes.Role, "HocVien")
//             };

//             var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//             await HttpContext.SignInAsync(
//                 CookieAuthenticationDefaults.AuthenticationScheme,
//                 new ClaimsPrincipal(claimsIdentity));

//             if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
//             {
//                 return Redirect(returnUrl);
//             }

//             return RedirectToAction("Index", "Home");
//         }

//         ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng");
//     }

//     return View(model);
// }