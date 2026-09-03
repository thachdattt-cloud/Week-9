using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentResponseDto>
{
    private readonly IStudentRepository _studentRepository;

    public UpdateStudentCommandHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentResponseDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentID);
        if (student == null)
        {
            throw new NotFoundException("khong tim thay sinh vien can sua");
        }

        student.FullName = request.Name;
        student.Gender = request.Gender;
        student.BirthDate = request.BirthDate;
        student.Email = request.Email;
        student.ClassID = request.ClassID;

        await _studentRepository.SaveChangesAsync();

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