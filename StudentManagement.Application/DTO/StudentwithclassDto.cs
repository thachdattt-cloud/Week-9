namespace StudentManagement.Application.DTO
{
    public class StudentWithClassDto
    {
        public int StudentID { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }
}