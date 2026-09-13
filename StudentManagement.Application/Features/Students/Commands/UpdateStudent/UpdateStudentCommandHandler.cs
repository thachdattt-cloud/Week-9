using AutoMapper;
using MediatR;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentResponseDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public UpdateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<StudentResponseDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentID);
        if (student == null)
        {
            throw new NotFoundException("khong tim thay sinh vien can sua");
        }

        _mapper.Map(request, student); // map đè field từ request vào student đã lấy ra

        await _studentRepository.SaveChangesAsync();

        return _mapper.Map<StudentResponseDto>(student);
    }
}