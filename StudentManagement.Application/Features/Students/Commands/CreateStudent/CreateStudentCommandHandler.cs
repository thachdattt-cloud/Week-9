using AutoMapper;
using MediatR;
using StudentManagement.Application.Features.Students.Commands.CreateStudent;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Model;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = _mapper.Map<Student>(request);

        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();

        return student.StudentID;
    }
}