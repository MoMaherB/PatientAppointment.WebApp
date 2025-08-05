using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;

namespace PatientAppointment.Infrastructure
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public void Add(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Appointment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Appointment GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
