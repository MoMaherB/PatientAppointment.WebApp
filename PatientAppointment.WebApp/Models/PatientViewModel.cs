using System.ComponentModel.DataAnnotations;

namespace PatientAppointment.WebApp.Models
{
    public class PatientViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Phone(ErrorMessage = "The phone number format is not valid.")]

        public string Phone { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Country { get; set; }

    }
}
