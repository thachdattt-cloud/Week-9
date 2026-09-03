// tuan3/models/Class.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Domain.Model
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        public string ClassCode { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;


        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}