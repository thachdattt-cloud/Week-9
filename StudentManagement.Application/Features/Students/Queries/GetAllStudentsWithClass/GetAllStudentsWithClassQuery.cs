using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Students.Queries.GetAllStudentsWithClass;

public class GetAllStudentsWithClassQuery : IRequest<List<StudentWithClassDto>>
{
}