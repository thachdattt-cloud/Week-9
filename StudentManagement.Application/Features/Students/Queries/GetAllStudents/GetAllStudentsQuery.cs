using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Students.Queries.GetAllStudents;

public class GetAllStudentsQuery : IRequest<List<StudentResponseDto>>
{
    public string? Keyword { get; set; }
}