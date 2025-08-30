# 📦 ASP.NET Advanced SoftUni 2024 Project

This is a fullstack web application built with ASP.NET Core, SQL Server. The project includes unit testing with Moq, Entity Framework Core for database access, and some frontend functionality using AJAX.

## 💻 Technologies Used

*   **Frontend:** HTML, CSS, JavaScript, AJAX
*   **Backend:** ASP.NET Core 8, C#
*   **Database:** SQL Server (via Entity Framework Core)
*   **Testing:** NUnit and Moq for unit tests
*   **Other Features:** Authentication, Authorization, CRUD operations

## 🚀 Getting Started

### Prerequisites

*   .NET 8 SDK or newer
*   SQL Server (LocalDB or full instance)
*   Visual Studio 2022 or Visual Studio Code
*   Connection string for your SQL Server

## 🔧 Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/YourUsername/YourRepoName.git
cd YourRepoName
```

### 2. Configure Database Connection

Open `appsettings.json` and add your SQL Server connection string under the `ConnectionStrings` section:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YourDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Run the Application

```bash
cd .\TechStoreApp.Web\
dotnet run
```

## 🧪 Testing

tests are implemented using **NUnit** and **Moq**. To run the tests:

```bash
cd .\TechStoreApp.Services.Tests\
dotnet test
```

## 🌐 Frontend AJAX Features

*   Dynamic data fetching and updates without page reload
*   CRUD operations integrated with backend APIs
*   Responsive and interactive UI

## ⚡ Project Features

*   User registration and authentication
*   Role-based authorization (e.g., admin, regular user)
*   CRUD operations for main entities
*   Unit testing with Moq for backend logic
*   AJAX for UX - QOL

## 📂 Project Structure

```
/TechStoreApp
  ├── /TechStoreApp.Common
  ├── /TechStoreApp.Data
  ├── /TechStoreApp.Data.Models
  ├── /TechStoreApp.Services.Data
  ├── /TechStoreApp.Services.Tests
  ├── /TechStoreApp.Web
  ├── /TechStoreApp.Web.Infrastructure
  ├── /TechStoreApp.Web.ViewModels
  └── /TechStoreApp.WebAPI
```

## 📌 URLs

*  **Web Application:** `https://localhost:7292`