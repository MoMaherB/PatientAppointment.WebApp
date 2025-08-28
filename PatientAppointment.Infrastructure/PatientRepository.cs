using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;

namespace PatientAppointment.Infrastructure
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;
        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Patient patient)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                
                String sql = "INSERT INTO Patients (FullName, Address, Phone, Gender, BirthDate, Country) VALUES (@FullName, @Address, @Phone, @Gender, @BirthDate, @Country);";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@FullName", patient.FullName);
                command.Parameters.AddWithValue("@Address", patient.Address);
                command.Parameters.AddWithValue("@Phone", patient.Phone);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
                command.Parameters.AddWithValue("@Country", patient.Country);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
           using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Patients WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Patient> GetAll()
        {
            List<Patient> patients = new List<Patient>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, FullName, Address, Phone, Gender, BirthDate, Country FROM Patients";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Address = reader["Address"].ToString(),
                            BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                            Country = reader["Country"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Phone = reader["Phone"].ToString()

                        });
                    }
                }
            }
            return patients;
        }

        public Patient GetByID(int id)
        {
            Patient patient = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, FullName, Address, Phone, Gender, BirthDate, Country FROM Patients WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        patient = new Patient
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Address = reader["Address"].ToString(),
                            BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                            Country = reader["Country"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Phone = reader["Phone"].ToString()

                        };
                    }
                }
            }

            return patient;
        }

        public void Update(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                string sql = "UPDATE Patients SET  FullName = @FullName, Address = @Address, Phone = @Phone, Gender = @Gender, BirthDate = @BirthDate, Country = @Country WHERE Id = @Id;";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@Id", patient.Id);
                command.Parameters.AddWithValue("@FullName", patient.FullName);
                command.Parameters.AddWithValue("@Address", patient.Address);
                command.Parameters.AddWithValue("@Phone", patient.Phone);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
                command.Parameters.AddWithValue("@Country", patient.Country);
                conn.Open();
                command.ExecuteNonQuery();

            }
        }

        public Patient GetByPhone(string phone)
        {
            Patient patient = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, FullName, Address, Phone, Gender, BirthDate, Country FROM Patients WHERE phone = @phone";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@phone", phone);

                conn.Open();

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        patient = new Patient
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Address = reader["Address"].ToString(),
                            BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                            Country = reader["Country"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Phone = reader["Phone"].ToString()
                        }; 
                    }
                }

            }
            return patient;
        }

        public IEnumerable<Patient> Search(string? name, string? phone, DateTime? birthDate, string? gender, string? country)
        {
            List<Patient> patients= new List<Patient>();
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phone) &&
                !birthDate.HasValue && string.IsNullOrEmpty(gender) && string.IsNullOrEmpty(country))
            {
                return patients;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sql = new System.Text.StringBuilder("SELECT Id, FullName, Address, Phone, Gender, BirthDate, Country FROM Patients WHERE 1=1");
                var command = new SqlCommand();

                if (!string.IsNullOrEmpty(name))
                {
                    sql.Append(" AND FullName LIKE @Name");
                    command.Parameters.AddWithValue("@Name", $"%{name}%");
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    sql.Append(" AND Phone LIKE @Phone");
                    command.Parameters.AddWithValue("@Phone", $"%{phone}%");
                }
                if (birthDate.HasValue)
                {
                    sql.Append(" AND CAST(BirthDate AS DATE) = @BirthDate");
                    command.Parameters.AddWithValue("@BirthDate", birthDate.Value.Date);
                }
                if (!string.IsNullOrEmpty(gender))
                {
                    sql.Append(" AND Gender = @Gender");
                    command.Parameters.AddWithValue("@Gender", gender);
                }
                if (!string.IsNullOrEmpty(country))
                {
                    sql.Append(" AND Country LIKE @Country");
                    command.Parameters.AddWithValue("@Country", $"%{country}%");
                }

                command.Connection = connection;
                command.CommandText = sql.ToString();

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                            Country = reader["Country"].ToString()
                        });
                    }
                }
            }
     

            return patients;
        }
    }
}
