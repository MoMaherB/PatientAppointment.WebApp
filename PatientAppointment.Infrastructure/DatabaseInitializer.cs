using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PatientAppointment.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IConfiguration configuration)
        {
            // This method now assumes the database already exists.
            // It connects directly to the application's database.
            string appConnectionString = configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(appConnectionString))
            {
                connection.Open();

                // Create Users table only if it doesn't already exist
                ExecuteNonQuery(connection,
                    @"IF OBJECT_ID('dbo.Users', 'U') IS NULL
                      CREATE TABLE dbo.Users (
                          Id INT PRIMARY KEY IDENTITY(1,1),
                          Username NVARCHAR(100) NOT NULL UNIQUE,
                          PasswordHash NVARCHAR(256) NOT NULL,
                          Role NVARCHAR(50) NOT NULL DEFAULT 'None'
                      );"
                );

                // Create Patients table only if it doesn't already exist
                ExecuteNonQuery(connection,
                    @"IF OBJECT_ID('dbo.Patients', 'U') IS NULL
                      CREATE TABLE dbo.Patients (
                          Id INT PRIMARY KEY IDENTITY(1,1),
                          FullName NVARCHAR(100) NOT NULL,
                          Address NVARCHAR(255) NULL,
                          Phone NVARCHAR(20) NULL,
                          Gender NVARCHAR(10) NULL,
                          BirthDate DATE NOT NULL,
                          Country NVARCHAR(50) NULL
                      );"
                );

                // Create Appointments table only if it doesn't already exist
                ExecuteNonQuery(connection,
                    @"IF OBJECT_ID('dbo.Appointments', 'U') IS NULL
                      CREATE TABLE dbo.Appointments (
                          Id INT PRIMARY KEY IDENTITY(1,1),
                          StartDateTime DATETIME2 NOT NULL,
                          EndDateTime DATETIME2 NOT NULL,
                          AppointmentType NVARCHAR(50) NOT NULL,
                          AppointmentStatus NVARCHAR(50) NOT NULL,
                          PatientId INT NOT NULL,
                          CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientId)
                          REFERENCES dbo.Patients(Id) ON DELETE CASCADE
                      );"
                );

                // Insert the main admin user only if they do not already exist
                ExecuteNonQuery(connection,
                    @"IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'admin@clinic.com')
                      INSERT INTO dbo.Users (Username, PasswordHash, Role)
                      VALUES (
                          'admin@clinic.com',
                          'AQAAAAIAAYagAAAAEB3m4XgT1hb5mM+ORQ5JVAVdD58PGgELA8oiu7DBiCdBOS2qDRgM/fd/ud2ki2xq0Q==',
                          'Admin'
                      );"
                );
            }
        }

        private static void ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }
    }
}