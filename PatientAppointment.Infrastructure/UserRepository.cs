using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PatientAppointment.Application.Interfaces;
using PatientAppointment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PatientAppointment.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO dbo.Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@Role", user.Role);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, Username, PasswordHash, Role FROM Users";
                var command = new SqlCommand(sql, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Role = reader["Role"].ToString()
                        });
                    }
                }
            }
            return users;
        }

        public User GetByUsername(string username)
        {
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, Username, PasswordHash, Role FROM Users WHERE Username = @Username";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            return user;
        }
    }
}
