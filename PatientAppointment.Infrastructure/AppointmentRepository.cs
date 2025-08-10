using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;

namespace PatientAppointment.Infrastructure
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;
        public AppointmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Appointment appointment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Appointments (PatientId, StartDateTime, EndDateTime, AppointmentType, AppointmentStatus) VALUES (@PatientId, @StartDateTime, @EndDateTime, @AppointmentType, @AppointmentStatus)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@StartDateTime", appointment.StartDateTime);
                command.Parameters.AddWithValue("@EndDateTime", appointment.EndDateTime);
                command.Parameters.AddWithValue("@AppointmentType", appointment.AppointmentType.ToString());
                command.Parameters.AddWithValue("@AppointmentStatus", appointment.AppointmentStatus.ToString());
                connection.Open();
                command.ExecuteNonQuery();

            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Appointments WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();

            }
        }

        public IEnumerable<Appointment> GetAll()
        {
            List<Appointment> appointments = new List<Appointment>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, PatientId, StartDateTime, EndDateTime, AppointmentType, AppointmentStatus FROM Appointments";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            StartDateTime = Convert.ToDateTime(reader["StartDateTime"]),
                            EndDateTime = Convert.ToDateTime(reader["EndDateTime"]),
                            AppointmentType = (AppointmentType)Enum.Parse(typeof(AppointmentType), reader["AppointmentType"].ToString()),
                            AppointmentStatus = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), reader["AppointmentStatus"].ToString())
                        });
                    }
                    return appointments;
                }
            }
        }

        public Appointment GetById(int id)
        {
            Appointment appointment = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, PatientId, StartDateTime, EndDateTime, AppointmentType, AppointmentStatus FROM Appointments WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        appointment = new Appointment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            StartDateTime = Convert.ToDateTime(reader["StartDateTime"]),
                            EndDateTime = Convert.ToDateTime(reader["EndDateTime"]),
                            AppointmentType = (AppointmentType)Enum.Parse(typeof(AppointmentType), reader["AppointmentType"].ToString()),
                            AppointmentStatus = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), reader["AppointmentStatus"].ToString())
                        };
                    }
                }
            }
            return appointment;
        }

        public void Update(Appointment appointment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Appointments SET PatientId = @PatientId, AppointmentType = @AppointmentType, AppointmentStatus = @AppointmentStatus WHERE Id = @Id;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", appointment.Id);
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@StartDateTime", appointment.StartDateTime);
                command.Parameters.AddWithValue("@EndDateTime", appointment.EndDateTime);
                command.Parameters.AddWithValue("@AppointmentType", appointment.AppointmentType.ToString());
                command.Parameters.AddWithValue("@AppointmentStatus", appointment.AppointmentStatus.ToString());
                connection.Open();
                command.ExecuteNonQuery();

            }

        }
    }
}
