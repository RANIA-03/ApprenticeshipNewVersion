using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Apprenticeship.DTO
{
    public class AssignmentDTO
    {
        public int assignmentId { get; set; }
        [Required]
        public string assignmentTitle { get; set; }
        [Required]
        public string assignmentDescription { get; set; }
        public string assignmentNotes { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [ValidateDateNotInPast(ErrorMessage = "Start date cannot be in the past.")]
        public DateTime startDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [ValidateEndDate(ErrorMessage = "End date must be in the present and greater than the start date.")]
        public DateTime endDate { get; set; }
        public int trainingId { get; set; }
        public List<int> objectiveIds { get; set; }
    }
    public class ValidateDateNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;

            // Subtract 5 minutes from the current time
            DateTime currentTimeMinus5Minutes = DateTime.Now.AddMinutes(-5);

            if (date < currentTimeMinus5Minutes)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
    public class ValidateEndDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime endDate = (DateTime)value;
            DateTime startDate = (DateTime)validationContext.ObjectType.GetProperty("startDate").GetValue(validationContext.ObjectInstance, null);

            if (endDate <= DateTime.Now || endDate <= startDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
