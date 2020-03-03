using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public abstract class TaskBaseViewModel : IValidatableObject
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Executors { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Completion date")]
        public DateTime CompletionDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Completion time")]
        public DateTime CompletionTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime completionDateTime = CompletionDate.AddHours(CompletionTime.Hour)
                                                    .AddMinutes(CompletionTime.Minute);

            if (completionDateTime < DateTime.Now)
            {
                yield return new ValidationResult("The completion date cannot be less than the current date!",
                    new[] { nameof(CompletionDate), nameof(CompletionTime) });
            }
        }
    }
}
