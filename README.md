# 🚀 Training Center Management Website

This is a web application project built with ASP.NET Core MVC, designed to manage the core operations of a training center. The system serves both **administrators** for management tasks and **students** for course enrollment and tracking.

## ✨ Core Features

The system is divided into functionalities for different user roles.

### 1. User Authentication
-   **Registration:** Allows new students to create an account.
-   **Login:** Provides secure login for both administrators and students.
-   **Role-based Access:** The user interface and available actions are tailored based on the user's role (Admin or Student).

### 2. Administrator Features
-   **Course Management:** Full CRUD (Create, Read, Update, Delete) functionality for all courses.
-   **Student & Enrollment Management:** View and manage the complete list of students and track enrollments for each course.
-   **Statistics & Reporting:** Access dashboards for monthly revenue and total student enrollment figures.

### 3. User (Student) Features
-   **Course Catalog:** Browse all available courses offered by the training center.
-   **Course Enrollment:** Register for desired courses directly through the platform.
-   **My Registered Courses:** Access a personal dashboard to view a list of all courses they have enrolled in.

## 🖼️ Feature Screenshots

| Feature Description | Screenshot |
| :--- | :--- |
| **Image 1:** *[Your feature description, e.g., User Dashboard Home]* | `./README_IMG/anh1.png` |
| **Image 8:** *[Your feature description, e.g., Course Enrollment Page (User)]* | `./README_IMG/anh8.png` |
| **Image 9:** *[Your feature description, e.g., My Registered Courses Page (User)]* | `./README_IMG/anh9.png` |
| **Image 2:** *[Your feature description, e.g., Course Management Page (CRUD)]* | `./README_IMG/anh2.png` |
| **Image 3:** *[Your feature description, e.g., Student Roster Page]* | `./README_IMG/anh3.png` |
| **Image 4:** *[Your feature description, e.g., Monthly Revenue Statistics Chart]* | `./README_IMG/anh4.png` |
| **Image 5:** *[Your feature description, e.g., Edit Student Details Form]* | `./README_IMG/anh5.png` |
| **Image 6:** *[Your feature description, e.g., Course Enrollment List]* | `./README_IMG/anh6.png` |
| **Image 7:** *[Your feature description, e.g., Monthly Revenue Statistics Chart]* | `./README_IMG/anh7.png` |

## 🛠️ Technology Stack

-   **Backend:**
    -   **Framework:** ASP.NET Core MVC (.NET 6/7/8)
    -   **Language:** C#
    -   **ORM:** Entity Framework Core
-   **Frontend:**
    -   HTML, CSS, JavaScript
    -   **Library:** Bootstrap
-   **Database:**
    -   Microsoft SQL Server
-   **Architecture:**
    -   Model-View-Controller (MVC)
    -   Utilizes ViewModels to efficiently pass data to Views.

## 🚀 Setup and Run Instructions

### Prerequisites
-   [.NET SDK](https://dotnet.microsoft.com/en-us/download) (version matching the project).
-   [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
-   An IDE like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/).

### Installation Steps

1.  **Clone the repository:**
    ```sh
    git clone [YOUR_REPOSITORY_URL]
    cd TrungTamDaoTao
    ```

2.  **Configure the Connection String:**
    -   Open the `appsettings.json` file.
    -   Locate the `ConnectionStrings` section and modify the `DefaultConnection` value to point to your SQL Server instance.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=YourDBName;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }
    ```

3.  **Update the Database:**
    -   Open a terminal or PowerShell in the project's root directory.
    -   Run the following command to apply the database migrations:
    ```sh
    dotnet ef database update
    ```
    *(If you don't have the EF tool, install it globally with: `dotnet tool install --global dotnet-ef`)*

4.  **Run the project:**
    -   **Option 1 (Using .NET CLI):**
        ```sh
        dotnet run
        ```
    -   **Option 2 (Using Visual Studio):**
        -   Open the `.sln` file in Visual Studio.
        -   Press `F5` or the green "Run" button to start the application.

5.  **Access the application:**
    -   Open your web browser and navigate to the `https://localhost:xxxx` or `http://localhost:yyyy` address shown in your terminal.

