using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommand : IRequest<StudentResponseDto>
{
    public int StudentID { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
    public int ClassID { get; set; }
}