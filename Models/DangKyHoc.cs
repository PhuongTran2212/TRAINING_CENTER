using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TrungTamDaoTao.Data;
namespace TrungTamDaoTao.Models
{

public class DangKyHoc
{
    public int HocVienId { get; set; }
    public HocVien HocVien { get; set; }

    public int KhoaHocId { get; set; }
    public KhoaHoc KhoaHoc { get; set; }

    public DateTime NgayDangKy { get; set; }
}
}