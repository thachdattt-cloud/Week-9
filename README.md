# Tuần 7 - Layer Architecture

Mục tiêu tuần: Refactor API theo kiến trúc phân lớp — Controller/AutoMapper/Exception/Middlewares/Module/Resources (Api) / Common / Context / Model / Repository / Services — hiểu Dependency Injection và trách nhiệm từng layer.
Sản phẩm chính của tuần: Student API version Layer Architecture.

---

## Ngày 1: Layer Architecture overview

**Deliverable:** Thư mục rõ ràng
**Đánh giá:** Review kiến trúc

### Nội dung học
- **Layer Architecture:** tổ chức code thành các tầng riêng biệt, mỗi tầng 1 trách nhiệm rõ ràng, tầng trên chỉ gọi xuống tầng liền kề bên dưới:
  - **Controller** — nhận request HTTP, gọi Service, trả response. Không chứa business logic, không query DB trực tiếp.
  - **Service** — chứa business logic, gọi Repository, không biết chi tiết DB hoạt động ra sao.
  - **Repository** — chỉ thao tác dữ liệu qua `AppDbContext`, không chứa business logic.
- **Vì sao cần tách lớp:** Controller làm tất cả (nhận request, validate, query DB, xử lý logic) vi phạm Single Responsibility Principle — khó test business logic mà không dựng cả HTTP request, khó tái sử dụng logic ở nơi khác.

### Việc đã làm
- Refactor cấu trúc thư mục: `Data/` → `Context/`, `models/` → `Model/`, tạo mới `Api/` (chứa `Controllers/`, `Exceptions/`, `Middlewares/`, `Module/`, `AutoMapper/`), `Common/` (gộp `ApiResponse/`, `Pagination/`), `Repository/` và `Services/` (mỗi thư mục có `Interfaces/` và `Implementations/` riêng biệt).
- Đổi namespace tương ứng cho từng nhóm file đã di chuyển, build lại sau mỗi bước nhỏ để dễ xác định lỗi — tránh di chuyển hàng loạt rồi mới build 1 lần.
- Xác nhận build sạch, chạy lại project, test qua Swagger — hành vi API không đổi so với trước khi tách thư mục.

### File/thư mục thay đổi chính
```
tuan3/
├── Api/
│   ├── Controllers/
│   ├── Exceptions/
│   ├── Middlewares/
│   ├── Module/
│   └── AutoMapper/
├── Common/
│   ├── ApiResponse.cs
│   └── Pagination/
├── Context/
│   └── AppDbContext.cs
├── Model/
│   ├── Student.cs, Class.cs, Subject.cs, StudentGrade.cs
├── Repository/
│   ├── Interfaces/
│   └── Implementations/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── DTO/
├── Validators/
└── Migrations/
```

---

## Ngày 2: Service layer

**Deliverable:** StudentService
**Đánh giá:** Code review

### Nội dung học
- **Service layer:** tầng trung gian giữa Controller và Repository, chứa business logic (tính `Age` từ `BirthDate`, validate `dto.Name` không rỗng, ném `NotFoundException`...).
- Service **không biết gì về HTTP** — không dùng `ActionResult`, không có `[HttpGet]` — vì nếu Service trả `ActionResult`, sẽ không tái sử dụng được cho môi trường không phải Web API (Console app, background job), khó test, và trộn lẫn trách nhiệm (quyết định HTTP status code là việc của Controller).
- **Controller không được query DB trực tiếp nữa** — mọi thao tác dữ liệu đi qua `IStudentService`.

### Việc đã làm
- Tạo `IStudentService` định nghĩa đủ phương thức khớp với các action của `StudentController` (`GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `GetPageAsync`, `GetAllWithClassAsync`, `GetGradesDetailAsync`).
- Tạo `StudentService` implement interface trên, chuyển toàn bộ business logic (mapping DTO, tính tuổi, validate, throw exception) từ `StudentController` cũ sang.
- Refactor `StudentController`: inject `IStudentService` thay vì `AppDbContext`, mỗi action chỉ còn gọi Service và đóng gói `ApiResponse`.

---

## Ngày 3: Repository layer

**Deliverable:** StudentRepository
**Đánh giá:** Review dependency

### Nội dung học
- **Repository layer:** tầng duy nhất "chạm" trực tiếp vào `AppDbContext`, chỉ thao tác dữ liệu (query, insert, save), không chứa business logic.
- **Interface tách biệt implementation** (`Repository/Interfaces/`, `Repository/Implementations/`) — Service chỉ phụ thuộc `IStudentRepository`, không biết class `StudentRepository` cụ thể. Lợi ích: nếu sau này cần đổi implementation (ví dụ thêm cache), chỉ cần sửa 1 dòng đăng ký DI, không đụng tới Service/Controller.
- Repository nên trả về **entity thuần túy** cho các thao tác cần tracking (`GetByIdAsync` dùng cho `Update`/`Delete`); nhưng với các thao tác chỉ đọc (đặc biệt có JOIN quan hệ), Repository có thể trả thẳng DTO qua projection để giữ lợi ích hiệu năng đã học ở Tuần 6 — đây là đánh đổi có chủ đích giữa tính thuần túy của layering và hiệu năng.

### Việc đã làm
- Tạo `IStudentRepository`/`StudentRepository`, ban đầu tách theo hướng "thuần túy" (mọi phương thức đọc trả `List<Student>`, Service tự map sang DTO).
- Qua vấn đáp, phát hiện điểm không nhất quán: `GetAllAsync` trả entity + map ở Service (kém tối ưu), trong khi `GetAllWithClassAsync` trả thẳng DTO qua projection (tối ưu) — cùng là thao tác đọc nhưng xử lý khác nhau không có lý do rõ ràng.
- Refactor lại cho nhất quán: `GetAllAsync`, `GetPageAsync` đổi sang trả `List<StudentResponseDto>` qua projection (dùng chung 1 biểu thức `Expression<Func<Student, StudentResponseDto>>` để tránh lặp code map); chỉ giữ entity thật ở `GetByIdAsync` (phục vụ `Update`/`Delete`, cần entity để EF Core tracking).
- `StudentService` cập nhật lại tương ứng — `GetAllAsync`/`GetPageAsync` giờ chỉ gọi thẳng Repository và trả kết quả, không tự map nữa.

---

## Ngày 4: Dependency Injection

**Deliverable:** DI configuration
**Đánh giá:** Mentor kiểm tra app chạy không lỗi startup

### Nội dung học
- **Dependency Injection (DI):** "tiêm" phụ thuộc vào class từ bên ngoài qua constructor, thay vì class tự `new` ra phụ thuộc của mình — giúp tách rời (decoupling), dễ test (mock được), quản lý vòng đời tập trung.
- **3 lifetime:** Transient (mỗi lần inject tạo instance mới), Scoped (1 instance dùng chung suốt 1 HTTP request), Singleton (1 instance duy nhất cho toàn ứng dụng).
- **`AppDbContext` luôn là Scoped** — vì đại diện 1 phiên làm việc với DB, không nên dùng chung giữa các request (rủi ro race condition khi nhiều request chạy song song), cũng không nên Singleton (dữ liệu request này lẫn với request khác). Vì `StudentRepository`/`StudentService` phụ thuộc `AppDbContext` (Scoped), nên cũng phải đăng ký Scoped — tránh lỗi **Captive Dependency** (service sống lâu hơn giữ tham chiếu tới service sống ngắn hơn).

### Việc đã làm
- Đăng ký trong `Program.cs`:
  ```csharp
  builder.Services.AddScoped<IStudentRepository, StudentRepository>();
  builder.Services.AddScoped<IStudentService, StudentService>();
  ```
- Build và chạy lại project — xác nhận không còn lỗi `Unable to resolve service`.
- Test toàn bộ endpoint qua Swagger, xác nhận hoạt động đúng sau khi hoàn tất chuỗi DI.

---

## Ngày 5: Checkpoint tuần 7



### Nội dung học
- Tổng hợp lại toàn bộ chuỗi Controller → Service → Repository → AppDbContext, đảm bảo hành vi API không đổi so với Tuần 6, chỉ cấu trúc code thay đổi.
- Tự đánh giá code style: naming convention, tránh toán tử 3 ngôi ngoại trừ trường hợp bắt buộc kỹ thuật (bên trong `Select()` LINQ), dọn code chết, tính nhất quán giữa các phương thức đọc/ghi.

### Việc đã làm
- Rà soát build sạch, test lại toàn bộ endpoint (`GetAll`, `GetById`, `Create`, `Update`, `Delete`, `GetPage`, `with-class`, `include-demo`, `grades-detail`).
- Xác nhận `StudentController` không còn tham chiếu `AppDbContext`, không còn `MapToDto` (đã chuyển hết vào Service).
- Ghi chú các "ngoại lệ có chủ đích" trong kiến trúc: `GetAllWithClassAsync`/`GetGradesDetailAsync` trả thẳng DTO từ Repository (phá lệ layering thuần túy) vì lý do hiệu năng — quyết định có cân nhắc, không phải sai sót.

--

## Sơ đồ luồng request (sau khi hoàn thành Tuần 7)

```
1. HTTP Request → StudentController.GetAll()
2. Controller gọi → IStudentService.GetAllAsync()
3. Service gọi → IStudentRepository.GetAllAsync()
4. Repository dùng → AppDbContext để query SQL Server (qua DTO projection)
5. Repository trả về → List<StudentResponseDto> cho Service
6. Service trả về → Controller
7. Controller đóng gói → ApiResponse<...> → trả về HTTP Response
```

Mỗi tầng chỉ biết tầng liền kề bên dưới: Controller không biết `AppDbContext` tồn tại; Repository không biết `ApiResponse` là gì.

## So sánh cách tiếp cận Repository: thuần túy vs projection

| | Trả entity thuần túy | Trả DTO qua projection |
|---|---|---|
| Khi nào dùng | Thao tác ghi (`Create`, `Update`, `Delete`) — cần entity để EF Core tracking | Thao tác đọc (`GetAll`, `GetPage`, endpoint quan hệ) — chỉ cần hiển thị dữ liệu |
| Hiệu năng | Kéo toàn bộ cột entity về RAM | SQL Server chỉ SELECT đúng cột cần dùng |
| Layering thuần túy | Đúng chuẩn (Repository không biết DTO) | Ngoại lệ có chủ đích, đánh đổi lấy hiệu năng |

## Cách test toàn bộ tuần
1. Đảm bảo database `StudentManagement` đã migrate đầy đủ (kế thừa từ Tuần 6).
2. Chạy project (`dotnet run` hoặc F5), xác nhận không lỗi startup DI.
3. Test qua Swagger hoặc file `.http`:
   - `GET /api/students`
   - `GET /api/students/{id}`
   - `GET /api/students/Page`
   - `GET /api/students/with-class`
   - `GET /api/students/grades-detail`
   - `POST /api/students`, `PUT /api/students/{id}`, `DELETE /api/students/{id}`
4. Đối chiếu kết quả giống hệt phiên bản Tuần 6 (chưa refactor) — xác nhận thay đổi kiến trúc không làm đổi hành vi.
