using System.ComponentModel.DataAnnotations;

namespace SecurePasswordMVC.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Salt { get; set; }
    }
}
