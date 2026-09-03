using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommand : IRequest<int>
{
    public string StudentCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
    public int ClassID { get; set; }
}