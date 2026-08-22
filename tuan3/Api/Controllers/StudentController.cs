using Microsoft.AspNetCore.Mvc;
using tuan3.Api.Exceptions;
using tuan3.Common.ApiResponse;
using tuan3.Common.Pagination;
using tuan3.DTO;
using tuan3.Services.Interfaces;

namespace tuan3.Api.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<StudentResponseDto>>>> GetAll([FromQuery] string? keyword)
        {
            var result = await _studentService.GetAllAsync(keyword);
            return Ok(ApiResponse<List<StudentResponseDto>>.Ok(result, "Lay danh sach thanh cong"));
        }

        [HttpGet("with-class")]
        public async Task<ActionResult<ApiResponse<List<StudentWithClassDto>>>> GetAllWithClass()
        {
            var result = await _studentService.GetAllWithClassAsync();
            return Ok(ApiResponse<List<StudentWithClassDto>>.Ok(result, "Lay danh sach sinh vien kem lop thanh cong"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> GetById([FromRoute] int id)
        {
            var result = await _studentService.GetByIdAsync(id);
            return Ok(ApiResponse<StudentResponseDto>.Ok(result, "Lay du lieu thanh cong"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> Create([FromBody] CreateStudentDto dto)
        {
            var response = await _studentService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                ApiResponse<StudentResponseDto>.Ok(response, "Tao moi thanh cong"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> Update([FromRoute] int id, [FromBody] UpdateStudentDto dto)
        {
            var result = await _studentService.UpdateAsync(id, dto);
            return Ok(ApiResponse<StudentResponseDto>.Ok(result, "Cap nhat thanh cong"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete([FromRoute] int id)
        {
            await _studentService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Ok(null!, "Xoa thanh cong"));
        }

        [HttpGet("Page")]
        public async Task<ActionResult<ApiResponse<PagedResult<StudentResponseDto>>>> GetPage([FromQuery] PaginationQuery query)
        {
            var result = await _studentService.GetPageAsync(query);
            return Ok(ApiResponse<PagedResult<StudentResponseDto>>.Ok(result, "danh sach thong tin"));
        }

        [HttpGet("grades-detail")]
        public async Task<ActionResult<ApiResponse<List<GradeDetailDto>>>> GetGradesDetail()
        {
            var result = await _studentService.GetGradesDetailAsync();
            return Ok(ApiResponse<List<GradeDetailDto>>.Ok(result, "Lay chi tiet diem thanh cong"));
        }

        [HttpGet("test-error-500")]
        public IActionResult testError()
        {
            throw new Exception("loi 500");
        }
    }
}