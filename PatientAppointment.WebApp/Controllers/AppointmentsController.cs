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
        public AppointmentsController(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }
        public IActionResult Index(DateTime? date)
        {
            DateTime selectedDate = date ?? DateTime.Today;
          
            List<Appointment> bookedAppointments = _appointmentRepository.GetAllByDateWithPatient(selectedDate).ToList();

            ViewData["SelectedDate"] = selectedDate.ToString("yyyy-MM-dd");

            return View(bookedAppointments);
        }

        public IActionResult QuickCreate()
        {
            List<Patient> patients = _patientRepository.GetAll().ToList();

            ViewData["Title"] = "Create New Appointment";
            ViewData["btn"] = "Create";
            ViewData["Action"] = "Create";
            ViewData["Patients"] = new SelectList(patients, "Id", "FullName");


            return PartialView("_AppointmentForm", new QuickAddViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult QuickCreate([FromBody]QuickAddViewModel quickAddViewModel)
        {
          
            if (ModelState.IsValid)
            {
                Appointment appointment = new Appointment
                {
                    PatientId = quickAddViewModel.PatientId,
                    AppointmentType = quickAddViewModel.AppointmentType,
                    StartDateTime = quickAddViewModel.StartDateTime,
                    EndDateTime = quickAddViewModel.StartDateTime.AddMinutes(30),
                    AppointmentStatus = AppointmentStatus.Scheduled

                };

                _appointmentRepository.Add(appointment);
                return Json(new { success = true, message = "Patient created successfully." });


            }
            return Json(new { success = false});

        }

        public IActionResult QuickUpdate(int Id)
        {
            List<Patient> patients = _patientRepository.GetAll().ToList();

            ViewData["Title"] = "Update Appointment";
            ViewData["btn"] = "Update";
            ViewData["Action"] = "Update";
            ViewData["Patients"] = new SelectList(patients, "Id", "FullName");

            Appointment appointment = _appointmentRepository.GetById(Id);
            if (appointment == null)
            {
                return NotFound();
            }
            QuickAddViewModel quickAddViewModel = new QuickAddViewModel
            {
                PatientId = appointment.PatientId,
                AppointmentStatus = appointment.AppointmentStatus,
                AppointmentType = appointment.AppointmentType
            };


            return PartialView("_AppointmentForm", quickAddViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QuickUpdate([FromBody] QuickAddViewModel quickAddViewModel)
        {

            if (ModelState.IsValid)
            {
                Appointment appointment = new Appointment
                {
                    Id = quickAddViewModel.Id,
                    PatientId = quickAddViewModel.PatientId,
                    AppointmentType = quickAddViewModel.AppointmentType,
                    StartDateTime = quickAddViewModel.StartDateTime,
                    EndDateTime = quickAddViewModel.StartDateTime.AddMinutes(30),
                    AppointmentStatus = quickAddViewModel.AppointmentStatus

                };

                _appointmentRepository.Update(appointment);
                return Json(new { success = true, message = "Patient created successfully." });


            }
            return Json(new { success = false });

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id)
        {
            Appointment appontiment = _appointmentRepository.GetById(Id);
            if (appontiment == null)
            {
                return Json(new { success = false, message = "appontiment not found." });
            }

            _appointmentRepository.Delete(Id);
            return Json(new { success = true, message = "Patient deleted successfully." });

        }


    }
}
