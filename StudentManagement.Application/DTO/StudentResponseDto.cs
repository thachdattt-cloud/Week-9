namespace StudentManagement.Application.DTO
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? StudentCode { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public int ClassID { get; set; }
    }
}