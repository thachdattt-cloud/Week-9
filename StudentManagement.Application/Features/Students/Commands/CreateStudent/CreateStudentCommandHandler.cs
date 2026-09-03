using MediatR;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Model;

namespace StudentManagement.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly IStudentRepository _studentRepository;

    public CreateStudentCommandHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = new Student
        {
            StudentCode = request.StudentCode,
            FullName = request.FullName,
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            Email = request.Email,
            ClassID = request.ClassID
        };

        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();

        return student.StudentID;
    }
}