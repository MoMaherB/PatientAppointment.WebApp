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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int Id, AppointmentStatus NewStatus)
        {
            Appointment appontiment = _appointmentRepository.GetById(Id);
            if (appontiment == null)
            {
                return Json(new { success = false, message = "appontiment not found." });
            }
            appontiment.AppointmentStatus = NewStatus;

            _appointmentRepository.Update(appontiment);
            return Json(new { success = true, message = "Patient updated successfully." });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public  IActionResult Delete(int Id)
        //{
        //    Appointment appontiment = _appointmentRepository.GetById(Id);
        //    if (appontiment == null)
        //    {
        //        return Json(new { success = false, message = "appontiment not found." });
        //    }

        //    _appointmentRepository.Delete(Id);
        //    return Json(new { success = true, message = "Patient deleted successfully." });

        //}

        
    }
}
