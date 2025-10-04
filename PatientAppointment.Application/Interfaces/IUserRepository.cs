using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientAppointment.Domain;

namespace PatientAppointment.Application.Interfaces
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        void Add(User user);
        IEnumerable<User> GetAll();
    }
}
