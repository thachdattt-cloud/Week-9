namespace StudentManagement.Application.DTO;

public class StudentPagingDto
{
    public int StudentID { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
    public string? ClassName { get; set; }
}