# Track Blood Donation

## Overview
Track Blood Donation is a web application designed to help blood banks and individual donors manage and track blood donation activities. It provides functionalities for user registration, logging blood donations, and an admin dashboard to manage the system. The project is built using **ASP.NET Core MVC** and **SQL Server** for database management, ensuring scalability and ease of use.

## Key Features
### User Registration & Login
- A secure and user-friendly authentication system that supports both donor and admin roles.
- Users can sign up, log in, and manage their profiles securely.

### Blood Donation Logging
- **Donors** can log their blood donations, specifying the date, blood type, and location.
- The system calculates donation eligibility based on the last donation date and the required waiting period for the donor to become eligible again.

### Donation History & Tracking
- Users can view their entire donation history, complete with donation dates, blood types, and total donations.
- A future eligibility feature helps users know when they can donate again based on medical guidelines.

### Admin Dashboard
- Administrators can view all users, blood donation records, and manage the platform efficiently.
- Admins can add, edit, or delete donor records, ensuring that data is up-to-date and accurate.
- Advanced reporting features allow admins to monitor donation trends and identify high-demand blood types.

### Foreign Key Relationships for Data Security
- Each donation is tied to a specific user via **AccountId**, ensuring user-specific data access.
- This architecture ensures that each user’s donation history is isolated and protected from unauthorized access.

### Multilingual Support
- The interface supports multiple languages, with a focus on **Arabic**, making it accessible to a broader audience.
- All labels and messages are customizable to cater to the language preferences of the users.

### Responsive Design
- The application is built using **Bootstrap**, ensuring a responsive and mobile-friendly interface.
- Users can access the platform from any device—desktops, tablets, or smartphones—without sacrificing usability.

## Technologies Used
- **ASP.NET Core MVC**: The core web framework for creating the structure and logic of the application.
- **Entity Framework Core**: Handles database operations through object-relational mapping (ORM), making it easier to interact with SQL Server.
- **SQL Server**: The primary database used to store user accounts, donation records, and application data.
- **ASP.NET Identity**: Provides secure user authentication and authorization, including password hashing and token-based authentication.
- **Bootstrap**: A popular CSS framework used for responsive design, ensuring the application is accessible across multiple devices.
- **jQuery & JavaScript**: For interactive elements on the frontend.

## Setup and Installation

### Prerequisites
Ensure you have the following installed before setting up the project:
- **.NET 8 SDK** or higher
- **SQL Server** (local or remote instance)
- **Visual Studio** (or any IDE compatible with .NET Core)
- **Git** for version control
