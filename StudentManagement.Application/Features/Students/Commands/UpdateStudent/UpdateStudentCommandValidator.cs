using FluentValidation;

namespace StudentManagement.Application.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ten khong duoc de trong");

        RuleFor(x => x.ClassID)
            .GreaterThan(0).WithMessage("Phai chon lop hop le");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email khong dung dinh dang")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}