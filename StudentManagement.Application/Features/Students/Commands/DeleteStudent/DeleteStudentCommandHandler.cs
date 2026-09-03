using MediatR;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.Application.Features.Students.Commands.DeleteStudent;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Unit>
{
    private readonly IStudentRepository _studentRepository;

    public DeleteStudentCommandHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentID);
        if (student == null)
        {
            throw new NotFoundException("khong tim thay sinh vien can xoa");
        }

        _studentRepository.Remove(student);
        await _studentRepository.SaveChangesAsync();

        return Unit.Value;
    }
}