namespace StudentManagement.Application.DTO
{
    public class UpdateStudentDto
    {
        public string Name { get; set; } = string.Empty;

        public bool? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Email { get; set; }

        public int ClassID { get; set; }
    }
}