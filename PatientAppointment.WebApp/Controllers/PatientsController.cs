using Microsoft.AspNetCore.Mvc;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
using PatientAppointment.WebApp.Models;

namespace PatientAppointment.WebApp.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientRepository _patientRepository;
        public PatientsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public IActionResult Index()
        {
            List<Patient> patients = _patientRepository.GetAll().ToList();
            return View(patients);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Create New Patient";
            ViewData["btn"] = "Create";
            ViewData["Action"] = "Create";
            return View("PatientForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientViewModel patientViewModel)
        {
            var existingPatient = _patientRepository.GetByPhone(patientViewModel.Phone);

            if (existingPatient != null)
            {
                ModelState.AddModelError("Phone", "A patient with this phone number already exists.");
            }
            ModelState.Remove(nameof(patientViewModel.Id));
            if (ModelState.IsValid)
            {
                Patient patient = new Patient
                {
                    FullName = patientViewModel.FullName,
                    Address = patientViewModel.Address,
                    BirthDate = patientViewModel.BirthDate,
                    Gender = patientViewModel.Gender,
                    Phone = patientViewModel.Phone,
                    Country = patientViewModel.Country
                };

                _patientRepository.Add(patient);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Create New Patient";
            ViewData["btn"] = "Create";
            ViewData["Action"] = "Create";
            return View("PatientForm", patientViewModel);

        }

        public IActionResult Edit(int Id)
        {
            Patient patient = _patientRepository.GetByID(Id);
            if (patient == null)
            {
                return NotFound();
            }
            PatientViewModel patientViewModel = new PatientViewModel
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                Country = patient.Country,
                Phone = patient.Phone,
                Gender = patient.Gender
            };

            ViewData["Title"] = $"Update ({patient.FullName}) Information";
            ViewData["btn"] = "Update";
            ViewData["Action"] = "Edit";
            return View("PatientForm", patientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientViewModel patientViewModel)
        {
            if (ModelState.IsValid)
            {
                Patient patient = new Patient
                {
                    Id = patientViewModel.Id,
                    FullName = patientViewModel.FullName,
                    Address = patientViewModel.Address,
                    BirthDate = patientViewModel.BirthDate,
                    Gender = patientViewModel.Gender,
                    Phone = patientViewModel.Phone,
                    Country = patientViewModel.Country
                };

                _patientRepository.Update(patient);
                return RedirectToAction(nameof(Index));
            }
            return View("PatientForm", patientViewModel);

        }

        public IActionResult Details(int Id)
        {
            Patient patient = _patientRepository.GetByID(Id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

     
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Patient patient = _patientRepository.GetByID(Id);
            if (patient == null)
            {
                return Json(new { success = false, message = "Patient not found." });
            }
            _patientRepository.Delete(Id);
            return Json(new { success = true, message = "Patient deleted successfully." });
        }
    }
}