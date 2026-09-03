using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.Application.Features.Students.Queries.GetStudentById;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentResponseDto>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentByIdQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentResponseDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentID);
        if (student == null)
        {
            throw new NotFoundException("khong tim thay sinh vien");
        }

        int age = 0;
        if (student.BirthDate.HasValue)
        {
            age = DateTime.Today.Year - student.BirthDate.Value.Year;
        }

        return new StudentResponseDto
        {
            Id = student.StudentID,
            Name = student.FullName,
            Age = age,
            StudentCode = student.StudentCode,
            Gender = student.Gender,
            Email = student.Email,
            ClassID = student.ClassID
        };
    }
}