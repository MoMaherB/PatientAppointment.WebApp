using System.ComponentModel.DataAnnotations;
using PatientAppointment.Domain;

namespace PatientAppointment.WebApp.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartDateTime { get; set; }
        [Required]
        [Display(Name = "End Time")]
        public DateTime EndDateTime { get; set; }
        [Required]
        [Display(Name = "Type")]
        public AppointmentType AppointmentType { get; set; }
        [Required]
        [Display(Name = "Status")]
        public AppointmentStatus AppointmentStatus { get; set; }
        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }
    }
}
