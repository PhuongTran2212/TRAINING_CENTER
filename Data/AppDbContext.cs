using Microsoft.EntityFrameworkCore;
using TrungTamDaoTao.Models;

namespace TrungTamDaoTao.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<KhoaHoc> KhoaHocs { get; set; }
    public DbSet<HocVien> HocViens { get; set; }
    public DbSet<DangKyHoc> DangKyHocs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DangKyHoc>()
            .HasKey(dk => new { dk.HocVienId, dk.KhoaHocId });

        modelBuilder.Entity<DangKyHoc>()
            .HasOne(dk => dk.HocVien)
            .WithMany(hv => hv.DangKyHocs)
            .HasForeignKey(dk => dk.HocVienId);

        modelBuilder.Entity<DangKyHoc>()
            .HasOne(dk => dk.KhoaHoc)
            .WithMany(kh => kh.DangKyHocs)
            .HasForeignKey(dk => dk.KhoaHocId);
    }
}
