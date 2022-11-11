using System.ComponentModel.DataAnnotations;

namespace SecurePasswordMVC.Models
{
    public class User
    {
        /// <summary>
        /// User Model. I used a code first method for creating the database. 
        /// With the use of PM add-migration console command. 
        /// </summary>
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public byte[]? Salt { get; set; }
    }
}
