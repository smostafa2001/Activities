# Activities

A simple ASP.NET Core Web API implementing **Clean Architecture** and **CQRS** to manage Activities. This project demonstrates clean, maintainable, and testable code structure using modern best practices.

---

## 🛠️ Tech Stack

- **.NET 9 / ASP.NET Core Web API**
- **Entity Framework Core**
- **CQRS with MediatR**
- **Clean Architecture**
- **xUnit for Unit Testing**
- **FluentValidation**
- **SQL Server**

---

## 🚀 How to Run

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/)
- SQL Server
- EF Core CLI Tools (install with `dotnet tool install --global dotnet-ef` if not already)

### 1. Clone the Repository

```bash
git clone https://github.com/smostafa2001/Activities.git
cd Activities
```

2. Build the Solution
```bash
dotnet build
```

3. Run the Application
```bash
dotnet run --project Activities.API
```

5. Access the API
The API will be available at:
```bash
https://localhost:7222/activities
```
Use tools like Postman to interact with the endpoints.
