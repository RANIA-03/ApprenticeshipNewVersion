using Apprenticeship.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Apprenticeship.DTO
{
    public class TrainingDTO
    {
        public int trainingId { get; set; }
        [Required]
        public string studentId { get; set; }
        [Required]
        public string teamLeaderId { get; set; }
        [Required]
        public string schoolSupervisorId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [ValidateDateNotInPast(ErrorMessage = "Start date cannot be in the past.")]
        public DateTime startDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [ValidateEndDate(ErrorMessage = "End date must be in the present and greater than the start date.")]
        public DateTime endDate { get; set; }
        public List<int> objectiveIds { get; set; }
    }
}
