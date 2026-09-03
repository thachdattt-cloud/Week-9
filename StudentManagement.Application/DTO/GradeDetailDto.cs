// StudentManagement.Application/DTO/GradeDetailDto.cs
namespace StudentManagement.Application.DTO
{
    public class GradeDetailDto
    {
        public string StudentCode { get; set; } = string.Empty;
        public string StudentFullName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public decimal? Mark { get; set; }
    }
}