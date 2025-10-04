# Patient Appointment Scheduling System

This is a comprehensive web application designed to manage patient appointments for a medical clinic. It was built from the ground up using ASP.NET Core MVC and pure ADO.NET, focusing on a professional, multi-layer architecture and a modern, interactive user experience.

The system allows authenticated users (clinic staff) to manage patient records and schedule, update, and cancel appointments through an intuitive daily schedule interface.

## Features

**Patient Management:** Full CRUD (Create, Read, Update, Delete) functionality for patient records with a responsive UI.

**Appointment Scheduling:** A dynamic, Ajax-driven daily schedule view.

- Create new appointments for specific patients in a modal without a page reload.  
- Update appointment status via a right-click context menu.  
- Delete appointments with a confirmation modal and a smooth fade-out effect.

**Search & Filtering:**

- Search for patients by name, phone, birth date, gender, or country.  
- Sort patient appointment lists by date.

**Security:** A robust cookie-based authentication and authorization system.

**Role-Based Access Control:**

- An Admin role with full access.  
- A Receptionist role for standard operations.

**Admin Dashboard:** A secure area for administrators to create and manage user accounts.

## Architecture

This project is built using the principles of Clean Architecture (also known as Onion Architecture) to ensure a strong Separation of Concerns. The solution is divided into four distinct layers:

**Domain Layer (PatientAppointment.Domain):**  
The core of the application. Contains the fundamental business entities (Patient, Appointment, User) and has zero dependencies on other layers.

**Application Layer (PatientAppointment.Application):**  
Defines the application's use cases and abstracts external concerns by using interfaces (e.g., IPatientRepository). It depends only on the Domain layer.

**Infrastructure Layer (PatientAppointment.Infrastructure):**  
Provides the concrete implementation of the interfaces defined in the Application layer. This is where all the database logic (pure ADO.NET code) and the DatabaseInitializer reside.

**Presentation Layer (PatientAppointment.WebApp):**  
The user-facing ASP.NET Core MVC project. It contains the Controllers, ViewModels, and Views. It depends on the Application layer for its logic.

This architecture makes the application maintainable, testable, and flexible for future changes.

## Technology Stack

**Backend:** C#, .NET 8, ASP.NET Core MVC  
**Data Access:** Raw ADO.NET, SQL Server  
**Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5, jQuery  
**Real-time UI:** Ajax (fetch API), Toastr.js for notifications  
**Design Patterns:** Clean Architecture, Repository Pattern, Dependency Injection

## Setup and Installation

### Prerequisites

- Visual Studio 2022  
- .NET 8 SDK  
- SQL Server (e.g., SQL Server Express LocalDB)

### Local Development Setup

The application is designed to be set up automatically on a new developer's machine.

**1. Clone the Repository**
**2. Configure the Connection String:**
- Open the PatientAppointment.WebApp/appsettings.json file.

- Find the DefaultConnection string.

- Update the Server and Database properties to point to your local SQL Server instance. For a default SQL Express installation, it will likely be Server=.\\SQLEXPRESS;.

**3. Run the Application:**
- Open the solution in Visual Studio.
- Set PatientAppointment.WebApp as the startup project.
- Uncomment the line `DatabaseInitializer.Initialize(connectionString);` in Program.cs to enable automatic database creation and seeding.
- Run the application (F5 or Ctrl+F5). The database will be created

### 4. Access the Application:**
- Open your web browser and navigate to https://localhost:5001 (or the URL provided by Visual Studio).
- Log in using the default admin credentials:
- Username: admin@clinic.com
- Password: Admin@123
- Pa$$w0rd.
- You can skip the database initialization if you let hte connection string point to the existing database.




