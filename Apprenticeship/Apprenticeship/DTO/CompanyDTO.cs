using System.ComponentModel.DataAnnotations;

namespace Apprenticeship.DTO
{
    public class CompanyDTO
	{
        public int companyId { get; set; }
        [Required]
        public string companyName { get; set; }
        [Required]
        public string companyAddress { get; set; }
    }
}
