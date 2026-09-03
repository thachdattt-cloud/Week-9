using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Application.DTO
{
    public class CreateStudentDto
    {
        public string Name { get; set; } = string.Empty;

        public string? StudentCode { get; set; }

        public bool? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Email { get; set; }

        public int ClassID { get; set; }
    }
}