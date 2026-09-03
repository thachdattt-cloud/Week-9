using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Features.Students.Commands.CreateStudent;
using StudentManagement.Application.Features.Students.Commands.DeleteStudent;
using StudentManagement.Application.Features.Students.Commands.UpdateStudent;
using StudentManagement.Application.Features.Students.Queries.GetAllStudents;
using StudentManagement.Application.Features.Students.Queries.GetStudentById;
using StudentManagement.Application.Features.Students.Queries.GetStudents;
using StudentManagement.Application.Features.Students.Queries.GetStudentsPagingDapper;
using StudentManagement.Shared.Common.ApiResponse;
using StudentManagement.Shared.Common.Pagination;
using StudentManagement.Application.Features.Students.Queries.GetAllStudentsWithClass;
using StudentManagement.Application.Features.Students.Queries.GetGradesDetail;

namespace StudentManagement.Application.Api.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
       
        private readonly IMediator _mediator;
        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
           
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<StudentResponseDto>>>> GetAll([FromQuery] string? keyword)
        {
            var result = await _mediator.Send(new GetAllStudentsQuery { Keyword = keyword });
            return Ok(ApiResponse<List<StudentResponseDto>>.Ok(result, "Lay danh sach thanh cong"));
        }

        [HttpGet("with-class")]
        public async Task<ActionResult<ApiResponse<List<StudentWithClassDto>>>> GetAllWithClass()
        {
            var result = await _mediator.Send(new GetAllStudentsWithClassQuery());
            return Ok(ApiResponse<List<StudentWithClassDto>>.Ok(result, "Lay danh sach sinh vien kem lop thanh cong"));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetStudentByIdQuery { StudentID = id });
            return Ok(ApiResponse<StudentResponseDto>.Ok(result, "Lay du lieu thanh cong"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CreateStudentCommand command)
        {
            var newId = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = newId },
                ApiResponse<int>.Ok(newId, "Tao moi thanh cong"));
        }

        /////
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> Update([FromRoute] int id, [FromBody] UpdateStudentDto dto)
        {
            var command = new UpdateStudentCommand
            {
                StudentID = id,
                Name = dto.Name,
                Gender = dto.Gender,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                ClassID = dto.ClassID
            };
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<StudentResponseDto>.Ok(result, "Cap nhat thanh cong"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteStudentCommand { StudentID = id });
            return Ok(ApiResponse<string>.Ok(null!, "Xoa thanh cong"));
        }

        [HttpGet("Page")]
        public async Task<ActionResult<ApiResponse<PagedResult<StudentResponseDto>>>> GetPage([FromQuery] PaginationQuery query)
        {
            var q = new GetStudentsQuery
            {
                Keyword = query.Keyword,
                PageNumber = query.PageNumber, 
                PageSize = query.PageSize
            };
            var result = await _mediator.Send(q);
            return Ok(ApiResponse<PagedResult<StudentResponseDto>>.Ok(result, "danh sach thong tin"));
        }

        [HttpGet("grades-detail")]
        public async Task<ActionResult<ApiResponse<List<GradeDetailDto>>>> GetGradesDetail()
        {
            var result = await _mediator.Send(new GetGradesDetailQuery());
            return Ok(ApiResponse<List<GradeDetailDto>>.Ok(result, "Lay chi tiet diem thanh cong"));
        }

        [HttpGet("page-dapper")]
        public async Task<ActionResult<ApiResponse<PagedResult<StudentPagingDto>>>> GetPageByDapper([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetStudentsPagingDapperQuery { PageIndex = pageIndex, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PagedResult<StudentPagingDto>>.Ok(result, "Lay danh sach phan trang thanh cong"));
        }

        [HttpGet("test-error-500")]
        public IActionResult testError()
        {
            throw new Exception("loi 500");
        }
    }
}