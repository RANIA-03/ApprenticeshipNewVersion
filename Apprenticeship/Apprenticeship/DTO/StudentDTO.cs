using System.ComponentModel.DataAnnotations;

namespace Apprenticeship.DTO
{
    public class StudentDTO
    {
        public string Id { get; set; }
        [Required]
        public string fristName { get; set; }
        [Required]
        public string secondName { get; set; }
        [Required]
        public string thirdName { get; set; }
        [Required]
		public string lastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
		public string studentMajor { get; set; }
		public int schoolId{ get; set; }
    }
}
