using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Domain.Model
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        [Key]
        public int RefreshTokenID { get; set; }

        public string Token { get; set; } = string.Empty;

        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; } = false;
    }
}