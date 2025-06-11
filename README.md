# Announcement Web API

Full-stack web application for managing announcements - test assignment for Junior .NET Developer position.

**Stack:** .NET 9 Web API + React + SQL Server

## 📌 Features
* CRUD operations for announcements
* Similar announcements detection using Full-Text Search (shared words in title/description)
* Responsive web interface
* Pagination, validation, error handling
* Unit tests

## 🛠️ Technologies

### Backend
* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server Express
* Mapster
* FluentValidation
* Swagger
* xUnit & NSubstitute

### Frontend
* React 19
* React Router
* Axios
* Bootstrap
* React Hooks

## 🚀 Quick Start

### Backend
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend  
```bash
cd frontend
npm install
npm start
```

**Access:** Frontend at http://localhost:5173, API docs at http://localhost:5188/swagger

## 🎯 Assignment Completed
✅ Add, edit, delete announcements  
✅ List announcements  
✅ Show details with top 3 similar announcements  
✅ Optional UI implemented

**Sample Data:** Run `database-setup.sql` to populate database with test data
