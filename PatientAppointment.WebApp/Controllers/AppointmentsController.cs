using Microsoft.AspNetCore.Mvc;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
using PatientAppointment.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
        public IActionResult Index(DateTime? date)
        {
            DateTime selectedDate = date ?? DateTime.Today;

            var bookedAppointments = _appointmentRepository.GetAllByDateWithPatient(selectedDate);

            ViewData["SelectedDate"] = selectedDate.ToString("yyyy-MM-dd");

            return View(bookedAppointments);
        }
    }
}
