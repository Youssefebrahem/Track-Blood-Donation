using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        [RegularExpression(@"[a-zA-Z0-9_]+@[a-z]+\.[a-zA-Z]{2,4}")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(20)] // You might want to increase the max length for security
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool Status { get; set; } = true;

        // Navigation property to link with Blood donations
        public virtual ICollection<Blood> BloodDonations { get; set; } = new List<Blood>();
    }
}
