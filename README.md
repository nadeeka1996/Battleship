# Battleship Application

A classic Battleship game built with **.NET 8 (Web API)** for the backend and **Angular 19** for the frontend.

---

## 🧱 Project Structure

```
/                         # Solution Root
├── API/                  # .NET 8 Backend (Web API)
│   ├── src/
│   │   ├── Battleship.Api            
│   │   ├── Battleship.Application     
│   │   ├── Battleship.Domain          
│   │   └── Battleship.Infrastructure 
│   └── test/
│       └── Battleship.Tests        
│
└── ClientApps/
    └── battleship-client             # Angular 19 Frontend
        ├── src/
        ├── angular.json
        └── package.json

---

## ⚙️ Prerequisites

### Backend (.NET 8)
- .NET 8 SDK
- Visual Studio 2022+ or VS Code

### Frontend (Angular 19)
- Node.js (v18+)
- Angular CLI (`npm install -g @angular/cli@19`)

---

## 🚀 How to Run the Project

### 1. Clone the Repository
```bash
git clone https://github.com/nadeeka1996/Battleship.git
cd Battleship
```

### 2. Run the Backend API
```bash
dotnet run --project API/Battleship.Api
```
API runs on [https://localhost:7034](https://localhost:7034) by default (check launch settings).

### 3. Setup and Run the Angular Frontend
```bash
cd ClientApps/battleship-client
npm install
ng serve --open
```
Frontend runs on [http://localhost:4200](http://localhost:4200).

---

## 🧪 Running Tests

To run backend tests:
```bash
dotnet test Battleship.Tests/Battleship.Tests.csproj
```
This will execute all unit tests for the backend.

---

## 📦 Technologies Used

### Backend
- .NET 8 (ASP.NET Core Web API)

### Frontend
- Angular 19
- Angular Material
- TypeScript

---

## ✅ Features
- Start a new Battleship game where the user plays against the computer in a single-sided game
- RESTful API endpoints (built with .NET 8 Web API)
- Modern Angular UI (Angular 19 frontend)

