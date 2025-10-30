// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using TrungTamDaoTao.Models;

// namespace TrungTamDaoTao.Controllers;

// public class HomeController : Controller
// {
//     private readonly ILogger<HomeController> _logger;

//     public HomeController(ILogger<HomeController> logger)
//     {
//         _logger = logger;
//     }

//     public IActionResult Index()
//     {
//         return View();
//     }

//     public IActionResult Privacy()
//     {
//         return View();
//     }

//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }
// }
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrungTamDaoTao.Data;
using TrungTamDaoTao.ViewModels;
using System.Security.Claims;

namespace TrungTamDaoTao.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get current user ID if logged in
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out int userId))
                {
                    currentUserId = userId;
                }
            }

            // Get featured courses (you could implement your own logic to determine which courses are "featured")
            var khoaHocs = await _context.KhoaHocs.ToListAsync();
            var danhSachKhoaHoc = new List<DangKyHocViewModel>();

            foreach (var khoaHoc in khoaHocs)
            {
                // Count current registrations
                int soLuongDaDangKy = await _context.DangKyHocs
                    .Where(dk => dk.KhoaHocId == khoaHoc.KhoaHocId)
                    .CountAsync();

                bool daDangKy = false;
                if (currentUserId.HasValue)
                {
                    daDangKy = await _context.DangKyHocs
                        .AnyAsync(dk => dk.KhoaHocId == khoaHoc.KhoaHocId && dk.HocVienId == currentUserId.Value);
                }

                // Check if the course can be canceled (before start date)
                bool coTheHuy = daDangKy && khoaHoc.ThoiGianKhaiGiang > DateTime.Now;

                danhSachKhoaHoc.Add(new DangKyHocViewModel
                {
                    KhoaHoc = khoaHoc,
                    SoLuongDaDangKy = soLuongDaDangKy,
                    SoLuongConLai = khoaHoc.SoLuongToiDa - soLuongDaDangKy,
                    DaDangKy = daDangKy,
                    CoTheHuy = coTheHuy
                });
            }

            // Optional: You can limit the number of courses shown on the homepage
            // danhSachKhoaHoc = danhSachKhoaHoc.Take(6).ToList();
            
            return View(danhSachKhoaHoc);
        }
    }
}