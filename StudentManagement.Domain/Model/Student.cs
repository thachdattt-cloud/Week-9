using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Domain.Model
{



    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        public string StudentCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public bool? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Email { get; set; }

        public int ClassID { get; set; }
        public Class Class { get; set; } = null!;




        public Student() { }
    }
}
