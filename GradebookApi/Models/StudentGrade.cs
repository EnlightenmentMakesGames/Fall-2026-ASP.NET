using System.ComponentModel.DataAnnotations;

namespace GradebookApi.Models {
    public class StudentGrade {
        [Required]
        public string Student { get; set; } = string.Empty;

        [Range(0,100)]
        public int Score { get; set; } = 0; //Unsure if we ever init this to anything, initing to 0 to be safe
    }
}