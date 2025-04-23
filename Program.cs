// using TrungTamDaoTao.Models;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.EntityFrameworkCore;
// using TrungTamDaoTao.Data;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Identity;
// var builder = WebApplication.CreateBuilder(args);

// // Cấu hình kết nối MySQL
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// // Thêm các dịch vụ MVC
// builder.Services.AddControllersWithViews();

// // Cấu hình xác thực cookie
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Account/DangNhap";
//         options.AccessDeniedPath = "/Account/TuChoi";
//         options.ExpireTimeSpan = TimeSpan.FromHours(2);
//     });

// var app = builder.Build();

// // Cấu hình HTTP request pipeline
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// // Tạo cơ sở dữ liệu và áp dụng các migrations nếu không tồn tại
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         context.Database.Migrate();
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "Đã xảy ra lỗi khi khởi tạo cơ sở dữ liệu.");
//     }
// }
// app.Run();
// var config = builder.Configuration;
// var taiKhoan = config["AdminAccount:TaiKhoan"];
// var matKhau = config["AdminAccount:MatKhau"];
// var hoTen = config["AdminAccount:HoTen"];

// var db = builder.Services.BuildServiceProvider().GetRequiredService<AppDbContext>();

// // Kiểm tra xem tài khoản admin đã tồn tại trong bảng HocViens chưa
// if (!db.HocViens.Any(h => h.TaiKhoan == taiKhoan))
// {
//     // Tạo tài khoản admin mà không cần cột VaiTro
//     var admin = new HocVien
//     {
//         HoTen = hoTen,
//         TaiKhoan = taiKhoan,
//         MatKhau = matKhau  // Bạn vẫn có thể để mật khẩu không mã hóa, nhưng không nên trong thực tế
//     };

//     db.HocViens.Add(admin);
//     db.SaveChanges();
// }
using TrungTamDaoTao.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TrungTamDaoTao.Data;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình kết nối MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Thêm các dịch vụ MVC
builder.Services.AddControllersWithViews();

// Cấu hình xác thực cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/DangNhap";
        options.AccessDeniedPath = "/Account/TuChoi";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

var app = builder.Build();

// Tạo cơ sở dữ liệu và áp dụng các migrations nếu không tồn tại
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        
        // Kiểm tra và tạo tài khoản admin nếu cần
        var config = builder.Configuration;
        var taiKhoan = config["AdminAccount:TaiKhoan"];
        var matKhau = config["AdminAccount:MatKhau"];
        var hoTen = config["AdminAccount:HoTen"];

        // Kiểm tra xem tài khoản admin đã tồn tại trong bảng HocViens chưa
        if (!context.HocViens.Any(h => h.TaiKhoan == taiKhoan))
        {
            // Tạo tài khoản admin mà không cần cột VaiTro
            var admin = new HocVien
            {
                HoTen = hoTen,
                TaiKhoan = taiKhoan,
                MatKhau = matKhau  // Bạn vẫn có thể để mật khẩu không mã hóa, nhưng không nên trong thực tế
            };

            context.HocViens.Add(admin);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Đã xảy ra lỗi khi khởi tạo cơ sở dữ liệu.");
    }
}

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
