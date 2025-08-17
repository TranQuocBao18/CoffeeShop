# .NET Modular Architecture

Below is the step-by-step guide to run the application locally.

**Prerequisites:**

1. Visual Studio V17.10.0 and .NET 8.0 (for .NET Aspire applications)

**Step 1:**

Let’s clone repository in git.

**Step 2:**


**Run and Debug**:

```
dotnet restore --packages .nuget
dotnet build --no-restore

dotnet new globaljson
dotnet --list-sdks
dotnet run --project "./Host/CoffeeShop.Host.BackOffice/CoffeeShop.Host.BackOffice.csproj"

dotnet run --project BE/Migrations/CoffeeShop.Migration/CoffeeShop.Migration.csproj identity
dotnet run --project BE/Migrations/CoffeeShop.Migration/CoffeeShop.Migration.csproj application
```

```

**Launch APIs**

- Api documents: http://localhost:9000/swagger/index.html

```

```
Authentication:
{
  "ipAddress": "127.0.0.1",
  "email": "superadmin@gmail.com",
  "password": "P@ssw0rd",
  "rememberMe": true
}
```
```

**References**:

- https://blog.ntechdevelopers.com/
