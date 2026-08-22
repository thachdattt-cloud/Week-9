// tuan3/models/StudentGrade.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace tuan3.Model
{
    [Table("StudentGrades")]
    public class StudentGrade
    {
      
        public int StudentID { get; set; }
        public int SubjectID { get; set; }

        public decimal? Mark { get; set; }

        public DateTime? ExamDate { get; set; }

       
        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}