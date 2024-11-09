using System.ComponentModel.DataAnnotations;

namespace Blood_Donation.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(30, MinimumLength =3)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
