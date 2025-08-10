using Microsoft.AspNetCore.Mvc;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
using PatientAppointment.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PatientAppointment.WebApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        public AppointmentsController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
      public IActionResult Index()
        {
            return View();
        }
    }
}
