using MediatR;

namespace StudentManagement.Application.Features.Students.Commands.DeleteStudent;

public class DeleteStudentCommand : IRequest<Unit>
{
    public int StudentID { get; set; }
}