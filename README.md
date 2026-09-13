# Auth Module — StudentManagement API (Tuần 9)

Module xác thực & phân quyền cho project `StudentManagement` (Clean Architecture, .NET 8), xây dựng trên nền CQRS/MediatR đã có từ Tuần 8. Module gồm: đăng nhập bằng JWT, refresh token có rotation, và authorization theo Role/Policy.

---

## 1. Tổng quan kiến trúc

Module tuân thủ đúng 5 project hiện có, không tạo project mới:

```
StudentManagement.Domain          → Entity (User, RefreshToken), Exception (UnauthorizedException)
StudentManagement.Application     → Interface (IPasswordHasher, IJwtTokenGenerator,
                                     IUserRepository, IRefreshTokenRepository),
                                     Feature (Login, RefreshToken, RevokeToken theo CQRS/MediatR), DTO
StudentManagement.Infrastructure  → Implementation (BCryptPasswordHasher, JwtTokenGenerator,
                                     UserRepository, RefreshTokenRepository), migrations
StudentManagement.API             → AuthController, cấu hình JWT Bearer + Swagger + Policy trong Program.cs
StudentManagement.Shared          → (không đổi — vẫn dùng ApiResponse<T> chung)
```

Nguyên tắc xuyên suốt: **Application không biết công nghệ cụ thể** (không biết BCrypt hay JWT library là gì) — chỉ định nghĩa interface. **Infrastructure** mới là nơi cài package và viết code thật, implement lại interface đó (Dependency Inversion, đồng nhất với cách `IStudentRepository` đã làm ở Tuần 8).

---

## 2. Cấu trúc file mới thêm ở Tuần 9

| Layer | File | Vai trò |
|---|---|---|
| Domain | `Model/User.cs` | Entity user: `UserID`, `Username`, `PasswordHash`, `Role` |
| Domain | `Model/RefreshToken.cs` | Entity refresh token: `RefreshTokenID`, `Token`, `UserID` (FK), `ExpiryDate`, `IsRevoked` |
| Domain | `Exceptions/UnauthorizedException.cs` | Ném khi xác thực/refresh/logout thất bại → middleware map sang 401 |
| Application | `Interfaces/IPasswordHasher.cs` | Hợp đồng hash/verify password |
| Application | `Interfaces/IJwtTokenGenerator.cs` | Hợp đồng sinh Access Token (JWT) và Refresh Token (chuỗi ngẫu nhiên) |
| Application | `Interfaces/Repositories/IUserRepository.cs` | Tìm `User` theo username |
| Application | `Interfaces/Repositories/IRefreshTokenRepository.cs` | CRUD cơ bản cho `RefreshToken` |
| Application | `Features/Auth/Commands/Login/*` | `LoginCommand`, `LoginCommandHandler` |
| Application | `Features/Auth/Commands/RefreshToken/*` | `RefreshTokenCommand`, `RefreshTokenCommandHandler` |
| Application | `Features/Auth/Commands/RevokeToken/*` | `RevokeTokenCommand`, `RevokeTokenCommandHandler` |
| Application | `DTO/LoginResultDto.cs` | `AccessToken`, `RefreshToken`, `Username`, `Role` |
| Application | `Mappings/StudentProfile.cs` | (Tuần 9 Ngày 1) AutoMapper: Command↔Entity, Entity→ResponseDto |
| Infrastructure | `Services/BCryptPasswordHasher.cs` | Implement `IPasswordHasher` bằng `BCrypt.Net-Next` |
| Infrastructure | `Services/JwtTokenGenerator.cs` | Implement `IJwtTokenGenerator` bằng `System.IdentityModel.Tokens.Jwt` |
| Infrastructure | `Repository/Implementations/UserRepository.cs` | Implement `IUserRepository` |
| Infrastructure | `Repository/Implementations/RefreshTokenRepository.cs` | Implement `IRefreshTokenRepository` |
| Infrastructure | `Context/AppDbContext.cs` | Thêm `DbSet<User>`, `DbSet<RefreshToken>`, seed 2 user test |
| Api | `Controllers/AuthController.cs` | `login`, `refresh-token`, `logout`, `me` |
| Api | `Program.cs` | `AddAuthentication().AddJwtBearer(...)`, `AddAuthorization` (Policy `CanManageStudents`), Swagger Security Scheme |
| Api | `appsettings.json` | Section `JwtSettings` |
| Api | `Module/ServiceRegistrationModule.cs` | Đăng ký DI cho toàn bộ interface/implementation ở trên |

---

## 3. Package cài thêm (Infrastructure)

```bash
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
```

---

## 4. Cấu hình `appsettings.json`

```json
"JwtSettings": {
  "SecretKey": "day-la-chuoi-bi-mat-toi-thieu-32-ky-tu-tro-len-de-an-toan",
  "Issuer": "StudentManagementAPI",
  "Audience": "StudentManagementClient",
  "ExpiryMinutes": 15,
  "RefreshTokenExpiryDays": 7
}
```

> `SecretKey` ở môi trường thật (production) **không được commit vào Git** — nên chuyển sang User Secrets hoặc biến môi trường. Giữ trong `appsettings.json` chỉ chấp nhận được ở phạm vi bài tập/local dev.

---

## 5. Cài đặt & chạy lần đầu

```bash
# 1. Restore package
dotnet restore

# 2. Chạy migration (Package Manager Console, Default Project = StudentManagement.Infrastructure,
#    Startup Project vẫn là StudentManagement.API)
Update-Database

# 3. Chạy API (F5 trong Visual Studio, hoặc)
dotnet run --project StudentManagement.API
```

Migration đã có sẵn trong repo (không cần tạo lại): `InitialCreate`, `AddUserTable`, `AddRefreshTokenTable`, `AddSecondTestUser`.

**Tài khoản test có sẵn (seed data):**

| Username | Password | Role |
|---|---|---|
| `admin` | `123456` | Admin |
| `user1` | `123456` | User |

---

## 6. Danh sách endpoint

| Method | Endpoint | Yêu cầu | Ghi chú |
|---|---|---|---|
| POST | `/api/auth/login` | Không | Trả `AccessToken` (15p) + `RefreshToken` (7 ngày) |
| POST | `/api/auth/refresh-token` | Không | Cấp cặp token mới, **rotation**: token cũ tự động bị revoke |
| POST | `/api/auth/logout` | Không | Revoke refresh token theo yêu cầu |
| GET | `/api/auth/me` | `[Authorize]` | Trả `UserId`/`Username`/`Role` đọc từ Claims trong token |
| GET | `/api/students`, `/api/students/{id}`, `/api/students/with-class` | `[Authorize]` | Admin + User đều gọi được |
| POST | `/api/students` | `[Authorize(Roles = "Admin")]` | Chỉ Admin |
| PUT | `/api/students/{id}` | `[Authorize(Roles = "Admin")]` | Chỉ Admin |
| DELETE | `/api/students/{id}` | `[Authorize(Policy = "CanManageStudents")]` | Chỉ Admin + có Claim `NameIdentifier` |

---

## 7. Luồng hoạt động

### 7.1 Login

```
Client → POST /api/auth/login {username, password}
  → LoginCommandHandler:
      1. Tìm User theo username (404 ẩn dưới dạng 401 nếu sai)
      2. Verify password bằng BCrypt
      3. Sinh Access Token (JWT, Claims: UserID/Username/Role, sống 15p)
      4. Sinh Refresh Token (chuỗi random 64 byte, không phải JWT)
      5. Lưu Refresh Token vào DB (UserID, ExpiryDate = +7 ngày, IsRevoked = false)
  → Trả AccessToken + RefreshToken cho Client
```

Sai username hoặc sai password đều trả **cùng 1 message chung** ("Sai username hoac password") để chống **User Enumeration**.

### 7.2 Refresh Token (kèm Rotation)

```
Client → POST /api/auth/refresh-token {refreshToken}
  → RefreshTokenCommandHandler:
      1. Tìm RefreshToken trong DB theo chuỗi token
      2. Kiểm tra: tồn tại? chưa bị revoke? còn hạn?
         → sai bất kỳ điều nào → 401
      3. Đánh dấu token cũ IsRevoked = true (ROTATION)
      4. Sinh cặp Access Token + Refresh Token MỚI
      5. SaveChangesAsync (1 lần, áp dụng cả Update token cũ + Insert token mới)
  → Trả cặp token mới
```

Rotation giới hạn thời gian hacker có thể lợi dụng nếu trộm được refresh token: token chỉ dùng được **đúng 1 lần**, dùng xong là hết giá trị.

### 7.3 Logout

```
Client → POST /api/auth/logout {refreshToken}
  → RevokeTokenCommandHandler:
      1. Tìm RefreshToken theo chuỗi token (không tồn tại → 401)
      2. IsRevoked = true (không kiểm tra hạn — dù còn hạn hay hết hạn đều revoke được)
      3. SaveChangesAsync
```

Không có `[Authorize]` — vì bản thân refresh token đã là bằng chứng đủ mạnh (64 byte ngẫu nhiên), và Access Token đã hết hạn có thể đúng lúc user muốn logout.

> **Giới hạn cần biết:** Access Token cũ (JWT) vẫn dùng được tới khi tự hết hạn (tối đa 15 phút) dù đã Logout — vì JWT là stateless, server không revoke tức thời được. Logout chỉ chặn được việc **refresh** tiếp, không thu hồi Access Token đang lưu hành ngay lập tức.

### 7.4 Authorization (Role / Policy)

```
Request → app.UseAuthentication()
    → đọc header Authorization: Bearer <token>
    → verify chữ ký + hạn → giải mã Claims → gán vào User.Identity
    → LUÔN cho request đi tiếp (kể cả không có token — lúc đó IsAuthenticated = false)
→ app.UseAuthorization()
    → so [Authorize] trên Action với User.Identity đã có
    → thiếu token / token sai / hết hạn → 401 Unauthorized
    → có token hợp lệ nhưng sai Role/Policy → 403 Forbidden
    → đủ điều kiện → cho vào Controller
```

Policy `CanManageStudents` (dùng cho `DELETE`) định nghĩa trong `Program.cs`:

```csharp
options.AddPolicy("CanManageStudents", policy =>
    policy.RequireRole("Admin")
          .RequireClaim(ClaimTypes.NameIdentifier));
```

---

## 8. Hướng dẫn test bằng Swagger

1. Mở Swagger UI, gọi `POST /api/auth/login` với `admin`/`123456` → copy giá trị `accessToken` trong response.
2. Bấm nút **Authorize** (góc trên bên phải, icon ổ khóa) → nhập `Bearer <accessToken>` (giữ đúng chữ `Bearer` + khoảng trắng) → Authorize → Close.
3. Gọi thử `GET /api/auth/me` → xác nhận đúng `UserId`/`Username`/`Role`.
4. Gọi `POST /api/students` (Create) → phải thành công (200) vì đang là Admin.
5. Bấm Authorize → Logout → login lại với `user1`/`123456` → Authorize lại bằng token mới.
6. Gọi `POST /api/students` với token `user1` → phải nhận **403 Forbidden**.
7. Test refresh: copy `refreshToken` từ 1 lần login bất kỳ → gọi `POST /api/auth/refresh-token` → nhận cặp token mới → gọi lại lần 2 với **token cũ** → phải nhận **401** (đã bị rotation revoke).
8. Test logout: copy `refreshToken` thật (không dùng giá trị mẫu `"string"` Swagger tự điền) → gọi `POST /api/auth/logout` → 200 → gọi lại `refresh-token` với token vừa logout → phải nhận **401**.

---

## 9. Lưu ý bảo mật đã áp dụng

- Password không bao giờ lưu dạng thô — chỉ lưu hash (BCrypt, tự động kèm Salt).
- Access Token sống ngắn (15 phút) để giới hạn thiệt hại nếu bị lộ; Refresh Token sống dài hơn (7 ngày) nhưng **lưu ở DB** nên revoke được.
- Refresh Token Rotation: mỗi lần dùng, token cũ bị vô hiệu hóa ngay, giảm cửa sổ thời gian có thể bị lợi dụng nếu bị đánh cắp.
- Message lỗi đăng nhập dùng chung 1 câu cho cả 2 trường hợp sai username/sai password, tránh lộ thông tin "username này có tồn tại hay không" (chống User Enumeration).
- Toàn bộ exception xác thực (`UnauthorizedException`) map thống nhất về HTTP 401 qua `GlobalExceptionMiddleware`.

