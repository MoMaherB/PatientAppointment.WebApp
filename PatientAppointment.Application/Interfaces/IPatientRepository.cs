using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientAppointment.Domain;

namespace PatientAppointment.Application.Interfaces
{
    public interface IPatientRepository
    {
        Patient GetByID(int id);
        IEnumerable<Patient> GetAll();
        void Add(Patient paitent);
        void Update(Patient paitent);
        void Delete(int id);
    }
}
