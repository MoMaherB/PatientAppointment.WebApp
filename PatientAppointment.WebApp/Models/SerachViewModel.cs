using System.ComponentModel.DataAnnotations;
using PatientAppointment.Domain;


namespace PatientAppointment.WebApp.Models
{
    public class SerachViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }

        public List<Patient> SearchResultPatients { get; set; } = new List<Patient>();
    }
}
