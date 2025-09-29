using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _2312764_NguyenKhiemThuan_ChuDe3
{
    public class Student
    {
        public string MSSV { get; set; }
        public string HoLot { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Lop { get; set; }
        public string SoCMND { get; set; }
        public string SoDT { get; set; }
        public string DiaChi { get; set; }
        public bool GioiTinh { get; set; } // true: Nam, false: Nữ
        public List<string> MonHocDangKy { get; set; }

        public Student()
        {
            MonHocDangKy = new List<string>();
        }
    }
    public class StudentManager
    {
        private List<Student> DanhSachSinhVien = new List<Student>();

        // Các phương thức chính - ĐÃ TRIỂN KHAI ĐẦY ĐỦ
        public void ThemSinhVien(Student sv)
        {
            // Kiểm tra trùng MSSV trước khi thêm
            if (DanhSachSinhVien.Any(s => s.MSSV == sv.MSSV))
            {
                throw new Exception($"MSSV {sv.MSSV} đã tồn tại!");
            }
            DanhSachSinhVien.Add(sv);
        }

        public void SuaSinhVien(Student sv)
        {
            // Tìm và cập nhật theo MSSV
            var existingStudent = DanhSachSinhVien.FirstOrDefault(s => s.MSSV == sv.MSSV);
            if (existingStudent != null)
            {
                // Cập nhật thông tin
                existingStudent.HoLot = sv.HoLot;
                existingStudent.Ten = sv.Ten;
                existingStudent.NgaySinh = sv.NgaySinh;
                existingStudent.Lop = sv.Lop;
                existingStudent.SoCMND = sv.SoCMND;
                existingStudent.SoDT = sv.SoDT;
                existingStudent.DiaChi = sv.DiaChi;
                existingStudent.GioiTinh = sv.GioiTinh;
                existingStudent.MonHocDangKy = sv.MonHocDangKy;
            }
            else
            {
                throw new Exception($"Không tìm thấy sinh viên với MSSV: {sv.MSSV}");
            }
        }

        public void XoaSinhVien(string mssv)
        {
            var studentToRemove = DanhSachSinhVien.FirstOrDefault(s => s.MSSV == mssv);
            if (studentToRemove != null)
            {
                DanhSachSinhVien.Remove(studentToRemove);
            }
            else
            {
                throw new Exception($"Không tìm thấy sinh viên với MSSV: {mssv}");
            }
        }

        public List<Student> TimKiem(string keyword)
        {
            // Tìm theo Tên, MSSV, Lớp (không phân biệt hoa thường)
            keyword = keyword.ToLower();
            return DanhSachSinhVien.Where(s =>
                s.MSSV.ToLower().Contains(keyword) ||
                s.Ten.ToLower().Contains(keyword) ||
                s.Lop.ToLower().Contains(keyword) ||
                $"{s.HoLot} {s.Ten}".ToLower().Contains(keyword)
            ).ToList();
        }

        // Property để truy cập danh sách từ bên ngoài (chỉ đọc)
        public List<Student> DS_SinhVien
        {
            get { return DanhSachSinhVien; }
        }

        // Đọc từ file TXT - ĐÃ TRIỂN KHAI ĐẦY ĐỦ
        public void DocTuFileTXT(string filePath)
        {
            DanhSachSinhVien.Clear();

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Không tìm thấy file: {filePath}");
            }

            string[] lines = File.ReadAllLines(filePath);

            // Bỏ qua dòng tiêu đề nếu có
            int startIndex = 0;
            if (lines.Length > 0 && lines[0].Contains("MSSV"))
            {
                startIndex = 1;
            }

            for (int i = startIndex; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split('\t'); // Giả sử dữ liệu phân tách bằng tab

                // Logic phân tích parts[] và tạo đối tượng Student
                if (parts.Length >= 8) // Đảm bảo có đủ thông tin
                {
                    Student sv = new Student
                    {
                        MSSV = parts[0],
                        HoLot = parts[1],
                        Ten = parts[2],
                        NgaySinh = DateTime.Parse(parts[3]),
                        Lop = parts[4],
                        SoCMND = parts[5],
                        SoDT = parts[6],
                        DiaChi = parts[7],
                        GioiTinh = parts[8] == "1" || parts[8].ToLower() == "nam",
                        MonHocDangKy = parts.Length > 9 ? parts[9].Split(',').ToList() : new List<string>()
                    };

                    DanhSachSinhVien.Add(sv);
                }
            }
        }

        // Ghi ra file TXT - ĐÃ TRIỂN KHAI ĐẦY ĐỦ
        public void GhiVaoFileTXT(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Ghi tiêu đề
                sw.WriteLine("MSSV\tHọ lót\tTên\tNgày sinh\tLớp\tSố CMND\tSố ĐT\tĐịa chỉ\tGiới tính\tMôn học");

                foreach (Student sv in DanhSachSinhVien)
                {
                    // Logic chuyển đổi đối tượng Student thành chuỗi
                    string monHoc = sv.MonHocDangKy != null ? string.Join(",", sv.MonHocDangKy) : "";
                    string gioiTinh = sv.GioiTinh ? "1" : "0";

                    string line = $"{sv.MSSV}\t{sv.HoLot}\t{sv.Ten}\t{sv.NgaySinh:dd/MM/yyyy}\t{sv.Lop}\t{sv.SoCMND}\t{sv.SoDT}\t{sv.DiaChi}\t{gioiTinh}\t{monHoc}";

                    sw.WriteLine(line);
                }
            }
        }

        // THÊM: Phương thức lấy sinh viên theo MSSV
        public Student GetStudentByMSSV(string mssv)
        {
            return DanhSachSinhVien.FirstOrDefault(s => s.MSSV == mssv);
        }

        // THÊM: Phương thức đếm tổng số sinh viên
        public int TongSoSinhVien()
        {
            return DanhSachSinhVien.Count;
        }
    }
}