# ASP.NET API Project - Learning Platform

## Overview
This is a demo ASP.NET Core API project for an online learning platform with email functionality for user registration and purchase notifications.

## ⚙️ Configuration Setup

### SMTP Email Configuration

**IMPORTANT:**  The SMTP server credentials have been removed from the `appsettings.json` file.

To test the email functionality, you need to configure your own SMTP settings:

#### Steps to Configure Email:

1. Navigate to the main project folder **`AlgorizaProject`**
2. Open `appsettings.json`
3. Locate the `EmailSettings` section
4. Add your SMTP credentials:

```json
{
  "EmailSettings": {
    "Server": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderPassword": "your-app-password",
    "EnableSsl": true,
    "Timeout": 60000
  }
}
```

#### Getting Gmail App Password:

If you're using Gmail, you need to generate an **App Password**:

1. Go to your Google Account settings
2. Enable 2-Factor Authentication (if not already enabled)
3. Go to Security → 2-Step Verification → App passwords
4. Generate a new app password for "Mail"
5. Use this 16-character password in the `SenderPassword` field in your `appsettings.json`



---

## 📊 Database Structure

### Entity Relationship Diagram

The following diagram shows the database schema and relationships between entities:

![Database ERD]

### Database Entities

#### **Users**
Main user accounts table for the learning platform
- **UserID** (PK) - Unique identifier
- UserName, UserEmail
- PasswordHash, PasswordSalt - Secure password storage
- FirstName, LastName
- TypeSign - User registration type
- CreateDateAndTime - Account creation timestamp

#### **Admins**
Administrative user accounts
- **adminID** (PK) - Unique identifier
- adminName, adminEmail
- PasswordHash, PasswordSalt
- CreateDateAndTime

#### **Instructors**
Course instructors/teachers
- **instructorID** (PK) - Unique identifier
- JobTitleID (FK) - Links to job titles
- instructorName, instructorDescription
- instructorImagePath - Profile image
- courseRate - Instructor rating
- CreateDateAndTime

#### **Courses**
Main course catalog
- **courseID** (PK) - Unique identifier
- courseName, courseDescription
- categoryID (FK) - Links to categories
- instructorID (FK) - Course instructor
- courseLevel, courseRate
- courseHours, coursePrice
- courseCertification
- courseImagePath
- CreateDateAndTime

#### **Categories**
Course categorization
- **categoryID** (PK) - Unique identifier
- categoryName, categoryDescription
- categoryImagePath

#### **Contents**
Course content/lectures
- **contentID** (PK) - Unique identifier
- courseID (FK) - Belongs to course
- contentName
- LecturesNumber, contentsHour
- CreateDateAndTime

#### **JobTitles**
Job titles for instructors
- **JobTitleID** (PK) - Unique identifier
- JobTitleName

#### **Payments**
Payment transactions
- **PaymentID** (PK) - Unique identifier
- UserID (FK) - User who made payment
- Country, State
- Amount, PaymentType
- CardName, CardNumber, CVV
- ExpiryDate, CreatedAt
- PaypalEmail

#### **UserCoursesHeaders**
User course enrollment header
- **UserCoursesHeaderID** (PK) - Unique identifier
- UserID (FK) - Enrolled user
- Total, Tax, Discount
- CreateDateAndTime

#### **UserCoursesDetails**
Details of user course purchases
- **UserCoursesDetailID** (PK) - Unique identifier
- UserCoursesHeaderID (FK) - Links to header
- courseID (FK) - Purchased course
- coursePrice

### Relationships

#### User Management
- **Users → Payments** (1:N) - One user can make many payments
- **Users → UserCoursesHeaders** (1:N) - One user can enroll in multiple courses

#### Course Structure
- **Categories → Courses** (1:N) - One category contains many courses
- **Courses → Contents** (1:N) - One course has many content items/lectures
- **Instructors → Courses** (1:N) - One instructor can teach multiple courses
- **JobTitles → Instructors** (1:N) - One job title can be assigned to many instructors

#### Enrollment & Purchases
- **UserCoursesHeaders → UserCoursesDetails** (1:N) - One enrollment header has many detail items
- **UserCoursesHeaders → Users** (N:1) - Many enrollments belong to one user
- **UserCoursesDetails → Courses** (N:1) - Multiple detail records can reference same course
- **Courses → UserCoursesDetails** (1:N) - One course can be purchased by many users

---

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 or higher
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone <your-repo-url>
```

2. Navigate to the project folder
```bash
cd AlgorizaProject
```

3. Restore dependencies
```bash
dotnet restore
```

4. Configure the database connection string in `appsettings.json`

5. Apply database migrations
```bash
dotnet ef database update
```

6. Configure SMTP settings (see SMTP Email Configuration section above)

7. Run the application
```bash
dotnet run
```

8. Access the API at `https://localhost:5001` or `https://markemad-001-site1.stempurl.com/swagger`

---

## 📧 Email Functionality

The API sends emails for the following events:

- **User Registration** - Welcome email with account confirmation
- **Course Purchase** - Order confirmation and receipt with course details

---

## 🔒 Security Notes

- SMTP credentials are not included in the repository
- All passwords are hashed using secure hashing algorithms with salt
- Ensure SSL/TLS is enabled for SMTP connections

---

## 📝 API Documentation

Access Swagger UI for complete API documentation at:
```
https://localhost:5001/swagger
```

Or can access Live Swagger UI at:
```
https://markemad-001-site1.stempurl.com/swagger
```

### Main API Endpoints

- **Authentication** - User login and registration
- **Courses** - Browse and manage courses
- **Instructors** - View instructor information
- **Categories** - Course categorization
- **Payments** - Process payments and enrollments
- **Admin** - Administrative functions

---

## 📄 License

No License - This is a demo project
