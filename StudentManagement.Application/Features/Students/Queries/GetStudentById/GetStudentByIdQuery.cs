using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Students.Queries.GetStudentById;

public class GetStudentByIdQuery : IRequest<StudentResponseDto>
{
    public int StudentID { get; set; }
}