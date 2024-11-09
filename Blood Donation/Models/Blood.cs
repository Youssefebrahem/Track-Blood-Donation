using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation.Models
{
    public class Blood
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DonationDate { get; set; }

        [StringLength(500)]
        public string? HealthStatus { get; set; }

        [Required]
        public string BloodReceiver { get; set; }

        // Removing static from RecentDonationDate to avoid it being shared across all Blood instances.
        public static DateTime RecentDonationDate { get; set; } = DateTime.MinValue;

        // Calculating days since last donation based on the DonationDate, if that’s intended.
        public int DaysSinceLastDonation => (DateTime.Now - DonationDate).Days;

        // Check eligibility based on the NextEligibleDonationDate
        public bool IsEligible => DateTime.Now >= NextEligibleDonationDate;

        [DataType(DataType.Date)]
        public DateTime NextEligibleDonationDate => DonationDate.AddDays(100);

        // Foreign key for the Account
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        // Navigation property to the Account/User
        public virtual Account Account { get; set; }

        public bool Status { get; set; } = true;
    }
}
