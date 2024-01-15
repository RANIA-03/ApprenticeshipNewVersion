using System.ComponentModel.DataAnnotations;

namespace Apprenticeship.DTO
{
    public class SchoolDTO
    {
        public int schoolId { get; set; }
        [Required]
        public string schoolName { get; set; }
        [Required]
        public string schoolAddress { get; set; }
        [Required]
        public string shortName { get; set; }

    }
}
