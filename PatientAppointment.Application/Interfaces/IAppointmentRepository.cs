using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientAppointment.Domain;

namespace PatientAppointment.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        Appointment GetById(int id);
        IEnumerable<Appointment> GetAll();
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int id);
        
    }
}
