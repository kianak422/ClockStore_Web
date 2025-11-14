using ClockStore.Models; // Import Model
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Để dùng hàm ToListAsync

namespace ClockStore.Controllers
{
    // Đảm bảo Controller này có thể xử lý các View
    public class ProductController : Controller 
    {
        // Khai báo Context
        private readonly StoreDbContext _db; 

        // Hàm khởi tạo (Constructor) để tiêm DbContext
        public ProductController(StoreDbContext db)
        {
            _db = db;
        }

        // =======================================================================
        // 1. CHỨC NĂNG ĐỌC (Xem Danh sách)
        // GET: /Product/Index
        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ danh sách Clocks từ CSDL
            IEnumerable<Clock> productList = await _db.Clocks.ToListAsync();
            
            // Trả danh sách này về View Index
            return View(productList);
        }

        // =======================================================================
        // 2. CHỨC NĂNG TẠO (Tạo mới)

        // GET: /Product/Create (Hiển thị Form)
        public IActionResult Create()
        {
            return View(); // Trả về View (form trống)
        }

        // POST: /Product/Create (Xử lý Form)
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(Clock obj)
        {
            // Kiểm tra tính hợp lệ của Model
            if (ModelState.IsValid)
            {
                await _db.Clocks.AddAsync(obj); // Thêm đối tượng vào bộ nhớ
                await _db.SaveChangesAsync(); // Lưu thay đổi vào CSDL
                TempData["success"] = "Sản phẩm đã được tạo mới thành công!"; 
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }
            return View(obj); 
        }

        // =======================================================================
        // 3. CHỨC NĂNG CẬP NHẬT (CHỈNH SỬA)

        // GET: /Product/Edit/{id} (Hiển thị Form Chỉnh sửa)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound(); 
            }

            // Tìm đối tượng Clock trong CSDL theo ID
            var productFromDb = await _db.Clocks.FindAsync(id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            // Trả về View cùng với đối tượng đã tìm thấy
            return View(productFromDb);
        }

        // POST: /Product/Edit (Xử lý Form Chỉnh sửa)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Clock obj)
        {
            // Kiểm tra tính hợp lệ của Model
            if (ModelState.IsValid)
            {
                _db.Clocks.Update(obj); // Đánh dấu đối tượng cần được cập nhật
                await _db.SaveChangesAsync(); // Lưu thay đổi vào CSDL

                TempData["success"] = "Sản phẩm đã được cập nhật thành công!";
                return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
            }
            return View(obj);
        }
        
        // =======================================================================
        // 4. CHỨC NĂNG XÓA (XÓA)

        // GET: /Product/Delete/{id} (Hiển thị xác nhận Xóa - 6A)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Tìm đối tượng Clock trong CSDL theo ID
            var productFromDb = await _db.Clocks.FindAsync(id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            
            // Trả về View xác nhận xóa
            return View(productFromDb);
        }

        // POST: /Product/Delete (Thực hiện Xóa - 6B)
        [HttpPost, ActionName("Delete")] // ActionName để View Delete gọi đúng hàm này
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            // Tìm đối tượng cần xóa
            var obj = await _db.Clocks.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Clocks.Remove(obj); // Xóa khỏi bộ nhớ
            await _db.SaveChangesAsync(); // Lưu thay đổi vào CSDL

            TempData["success"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction("Index"); // Chuyển hướng về trang danh sách
        }
    }
}
