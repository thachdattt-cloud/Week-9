// tuan3/models/Subject.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Domain.Model
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public int SubjectID { get; set; }

        public string SubjectCode { get; set; } = string.Empty;

        public string SubjectName { get; set; } = string.Empty;

        public int? Credits { get; set; }

        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>(); 
    }
}