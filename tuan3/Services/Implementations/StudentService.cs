using tuan3.Api.Exceptions;
using tuan3.Common.ApiResponse;
using tuan3.Common.Pagination;
using tuan3.DTO;
using tuan3.Model;
using tuan3.Repository.Interfaces;
using tuan3.Services.Interfaces;

namespace tuan3.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        // Chi con dung cho GetByIdAsync (tra ve tu entity that,
        // vi endpoint GetById co the tai su dung entity cho cac
        // muc dich khac ngoai hien thi don thuan).
        private StudentResponseDto MapToDto(Student student)
        {
            int age = 0;
            if (student.BirthDate.HasValue)
            {
                age = DateTime.Today.Year - student.BirthDate.Value.Year;
            }

            var dto = new StudentResponseDto();
            dto.Id = student.StudentID;
            dto.Name = student.FullName;
            dto.Age = age;
            dto.StudentCode = student.StudentCode;
            dto.Gender = student.Gender;
            dto.Email = student.Email;
            dto.ClassID = student.ClassID;

            return dto;
        }

        public async Task<List<StudentResponseDto>> GetAllAsync(string? keyword)
        {
            return await _repository.GetAllAsync(keyword);
        }

        public async Task<StudentResponseDto> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                throw new NotFoundException("khong tim thay sinh vien");
            }

            return MapToDto(student);
        }

        public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new BadRequestException("ten khong duoc de trong");
            }

            var newStudent = new Student();
            newStudent.FullName = dto.Name;
            newStudent.Gender = dto.Gender;
            newStudent.BirthDate = dto.BirthDate;
            newStudent.Email = dto.Email;
            newStudent.ClassID = dto.ClassID;

            if (!string.IsNullOrWhiteSpace(dto.StudentCode))
            {
                newStudent.StudentCode = dto.StudentCode;
            }

            await _repository.AddAsync(newStudent);
            await _repository.SaveChangesAsync();

            return MapToDto(newStudent);
        }

        public async Task<StudentResponseDto> UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                throw new NotFoundException("khong tim thay sinh vien can sua");
            }

            student.FullName = dto.Name;
            student.Gender = dto.Gender;
            student.BirthDate = dto.BirthDate;
            student.Email = dto.Email;
            student.ClassID = dto.ClassID;

            await _repository.SaveChangesAsync();

            return MapToDto(student);
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                throw new NotFoundException("khong tim thay sinh vien can xoa");
            }

            _repository.Remove(student);
            await _repository.SaveChangesAsync();
        }

        public async Task<PagedResult<StudentResponseDto>> GetPageAsync(PaginationQuery query)
        {
            var totalItems = await _repository.CountAsync(query.Keyword);
            var skipCount = (query.PageNumber - 1) * query.PageSize;

            var items = await _repository.GetPageAsync(query.Keyword, skipCount, query.PageSize);

            var pageResult = new PagedResult<StudentResponseDto>();
            pageResult.Items = items;
            pageResult.PageNumber = query.PageNumber;
            pageResult.PageSize = query.PageSize;
            pageResult.TotalItems = totalItems;

            return pageResult;
        }

        public async Task<List<StudentWithClassDto>> GetAllWithClassAsync()
        {
            return await _repository.GetAllWithClassAsync();
        }

        public async Task<List<GradeDetailDto>> GetGradesDetailAsync()
        {
            return await _repository.GetGradesDetailAsync();
        }
    }
}