using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Students.Queries.GetGradesDetail;

public class GetGradesDetailQuery : IRequest<List<GradeDetailDto>>
{
}