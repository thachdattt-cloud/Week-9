using AutoMapper;
using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Features.Students.Queries.GetStudentById;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentResponseDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetStudentByIdQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<StudentResponseDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentID);
        if (student == null)
            throw new NotFoundException("khong tim thay sinh vien");

        return _mapper.Map<StudentResponseDto>(student);
    }
}