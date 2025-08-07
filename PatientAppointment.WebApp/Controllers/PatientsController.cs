using Microsoft.AspNetCore.Mvc;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
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
            return View();
        }

    }
}
