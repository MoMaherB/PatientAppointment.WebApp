using PatientAppointment.Domain;
using System.ComponentModel.DataAnnotations;

namespace PatientAppointment.WebApp.Models
{
    public class QuickAddViewModel
    {
        public int Id { get;  set;}
        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "Appointment Type")]
        public AppointmentType AppointmentType { get; set; }

        [Required]
        [Display(Name = "Appointment Status")]
        public AppointmentStatus AppointmentStatus {get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
    }
}