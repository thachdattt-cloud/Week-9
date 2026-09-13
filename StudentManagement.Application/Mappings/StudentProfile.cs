using AutoMapper;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Features.Students.Commands.CreateStudent;
using StudentManagement.Application.Features.Students.Commands.UpdateStudent;
using StudentManagement.Domain.Model;

namespace StudentManagement.Application.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<CreateStudentCommand, Student>();
            // field trùng tên 100% (StudentCode, FullName, Gender, BirthDate, Email, ClassID) → không cần ForMember

            CreateMap<UpdateStudentCommand, Student>() 
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StudentID, opt => opt.Ignore()); // giữ nguyên StudentID gốc, không ghi đè

            CreateMap<Student, StudentResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.BirthDate.HasValue ? DateTime.Today.Year - src.BirthDate.Value.Year : 0));
        }
    }
}