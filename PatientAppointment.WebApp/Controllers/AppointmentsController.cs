using Microsoft.AspNetCore.Mvc;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
using PatientAppointment.WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace PatientAppointment.WebApp.Controllers
{
    [Authorize]
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

        public IActionResult QuickCreate([FromBody] QuickAddViewModel quickAddViewModel)
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
            return Json(new { success = false });

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
                return Json(new { success = true, message = "Patient updated successfully." });


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
            return Json(new { success = true, message = "Status updated successfully." });
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

        public IActionResult FindPatients()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FindPatients(SerachViewModel serachViewModel)
        {
            string? name = serachViewModel.FullName;
            string? phone = serachViewModel.Phone;
            DateTime? birthDate = serachViewModel.BirthDate != default ? serachViewModel.BirthDate : null;
            string gender = serachViewModel.Gender;
            string? country = serachViewModel.Country;

            List<Patient> searchResultPatients = _patientRepository.Search(name, phone, birthDate, gender, country).ToList();
            serachViewModel.SearchResultPatients = searchResultPatients;
            return View(serachViewModel);
        }

        public IActionResult PatientAppointments(int pid)
        {
            Patient patient = _patientRepository.GetByID(pid);
            List<Appointment> patientAppointments = _appointmentRepository.GetByPatientId(pid).ToList();
            patientAppointments = patientAppointments.OrderByDescending(a => a.StartDateTime).ToList();

            ViewData["patient"] = patient;

            return View(patientAppointments);
        }

        public IActionResult AddPatientAppointment(int pid)
        {
            Patient patient = _patientRepository.GetByID(pid);
            ViewData["patient"] = patient;
            ViewData["action"] = "AddPatientAppointment";
            QuickAddViewModel quickAddViewModel = new QuickAddViewModel();
            quickAddViewModel.StartDateTime = DateTime.Today;
            ViewData["actionTitle"] = "Create";
            return View("PatientAppointmentForm", quickAddViewModel) ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPatientAppointment(QuickAddViewModel quickAddViewModel)
        {
            if (ModelState.IsValid)
            {
                if (quickAddViewModel.StartDateTime == DateTime.MinValue)
                {
                    ModelState.AddModelError("StartDateTime", "Please select a valid date and time for the appointment.");
                }
                Appointment appointment = new Appointment
                {
                    PatientId = quickAddViewModel.PatientId,
                    AppointmentType = quickAddViewModel.AppointmentType,
                    StartDateTime = quickAddViewModel.StartDateTime,
                    EndDateTime = quickAddViewModel.StartDateTime.AddMinutes(30),
                    AppointmentStatus = AppointmentStatus.Scheduled
                };
                _appointmentRepository.Add(appointment);
                TempData["success"] = "Appointment created successfully.";
                return RedirectToAction("PatientAppointments", new { pid = quickAddViewModel.PatientId });
            }
            Patient patient = _patientRepository.GetByID(quickAddViewModel.PatientId);
            ViewData["patient"] = patient;
            TempData["error"] = "Please select a valid date and time for the appointment.";
            quickAddViewModel.StartDateTime = DateTime.Today;
            ViewData["action"] = "AddPatientAppointment";
            ViewData["actionTitle"] = "Create";

            return View("PatientAppointmentForm", quickAddViewModel);

        }

        public IActionResult UpdatePatientAppointment(int id)
        {
            Appointment appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            Patient patient = _patientRepository.GetByID(appointment.PatientId);
            QuickAddViewModel quickAddViewModel = new QuickAddViewModel
            {
                Id = id,
                PatientId = appointment.PatientId,
                AppointmentType = appointment.AppointmentType,
                StartDateTime = appointment.StartDateTime,
                AppointmentStatus = appointment.AppointmentStatus

            };

            ViewData["patient"] = patient;
            ViewData["action"] = "UpdatePatientAppointment";
            ViewData["actionTitle"] = "Update";

            return View("PatientAppointmentForm", quickAddViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePatientAppointment(QuickAddViewModel quickAddViewModel)
        {
            if (quickAddViewModel.StartDateTime == DateTime.MinValue)
            {
                ModelState.AddModelError("StartDateTime", "Please select a valid date and time for the appointment.");
                TempData["error"] = "Please select a valid date and time for the appointment.";

            }

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

                _appointmentRepository.UpdateFromForm(appointment);

                return RedirectToAction("PatientAppointments", new { pid = quickAddViewModel.PatientId });
            }

            Patient patient = _patientRepository.GetByID(quickAddViewModel.PatientId);
            ViewBag.patient = patient;
            ViewData["action"] = "UpdatePatientAppointment";
            quickAddViewModel.StartDateTime = DateTime.Today;
            ViewData["actionTitle"] = "Update";
            return View("PatientAppointmentForm", quickAddViewModel);
        }



        public IActionResult ShowAvailableSlots(DateTime? date)
        {

            DateTime selectedDate = date ?? DateTime.Today;

            List<Appointment> bookedAppointments = _appointmentRepository.GetAllByDateWithPatient(selectedDate).ToList();

            ViewData["SelectedDate"] = selectedDate.ToString("yyyy-MM-dd");

            return PartialView("_ShowAvailableSlots", bookedAppointments);
        }


    }
}
