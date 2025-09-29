using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2312764_NguyenKhiemThuan_ChuDe3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public class StudentInfo
        {
            // Các thuộc tính - SỬA: Thêm đủ các thuộc tính từ JSON
            public string MSSV { get; set; }
            public string Hoten { get; set; }  // SỬA: Đổi từ Hotenlot thành Hoten để khớp JSON
            public int Tuoi { get; set; }      // THÊM: Thuộc tính này có trong JSON
            public double Diem { get; set; }   // THÊM: Thuộc tính này có trong JSON
            public bool TonGiao { get; set; }  // THÊM: Thuộc tính này có trong JSON

            // Phương thức tạo lập - SỬA: Sửa tham số cho khớp
            public StudentInfo(string mssv, string hoten, int tuoi, double diem, bool tongiao)
            {
                this.MSSV = mssv;
                this.Hoten = hoten;
                this.Tuoi = tuoi;
                this.Diem = diem;
                this.TonGiao = tongiao;
            }

            // THÊM: Constructor mặc định (không tham số) để Json.NET có thể hoạt động
            public StudentInfo() { }
        }

        private void btnReadJSON_Click(object sender, EventArgs e)
        {
            try
            {
                // Đường dẫn tập tin JSON
                string Path = "../../students.json";

                // Kiểm tra file có tồn tại không
                if (!File.Exists(Path))
                {
                    MessageBox.Show("Không tìm thấy file students.json!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi phương thức LoadJSON để đọc danh sách sinh viên
                List<StudentInfo> list = LoadJSON(Path);

                // Xóa các mục cũ trong ListView trước khi thêm dữ liệu mới
                lvDanhSachSinhVien.Items.Clear();

                // Đảm bảo ListView có đủ cột
                if (lvDanhSachSinhVien.Columns.Count == 0)
                {
                    lvDanhSachSinhVien.Columns.Add("MSSV", 100);
                    lvDanhSachSinhVien.Columns.Add("Họ tên", 150);
                    lvDanhSachSinhVien.Columns.Add("Tuổi", 50);
                    lvDanhSachSinhVien.Columns.Add("Điểm", 70);
                    lvDanhSachSinhVien.Columns.Add("Tôn giáo", 80);
                }

                // Duyệt qua danh sách sinh viên
                foreach (StudentInfo info in list)
                {
                    // Tạo một ListViewItem mới với MSSV là mục chính
                    ListViewItem item = new ListViewItem(info.MSSV);

                    // Thêm các thông tin khác vào các cột phụ (sub-items)
                    item.SubItems.Add(info.Hoten);
                    item.SubItems.Add(info.Tuoi.ToString());
                    item.SubItems.Add(info.Diem.ToString());
                    item.SubItems.Add(info.TonGiao ? "Có" : "Không");

                    // Thêm mục đã tạo vào ListView
                    lvDanhSachSinhVien.Items.Add(item);
                }

                MessageBox.Show($"Đã tải {list.Count} sinh viên từ file JSON!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file JSON: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Phương thức đọc tập tin JSON - SỬA: Sử dụng cách đơn giản hơn với JsonConvert
        /// </summary>
        /// <param name="Path">Đường dẫn tập tin</param>
        /// <returns>Danh sách các đối tượng từ tập tin JSON</returns>
        private List<StudentInfo> LoadJSON(string Path)
        {
            try
            {
                // Đọc toàn bộ nội dung file
                string json = File.ReadAllText(Path);

                // Parse JSON thành JObject
                JObject jsonObject = JObject.Parse(json);

                // Lấy mảng "sinhvien" và chuyển thành danh sách StudentInfo
                List<StudentInfo> studentList = jsonObject["sinhvien"].ToObject<List<StudentInfo>>();

                return studentList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi phân tích JSON: {ex.Message}");
            }
        }
    }
}