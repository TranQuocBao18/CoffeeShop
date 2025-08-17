# .NET Modular Architecture

Below is the step-by-step guide to run the application locally.

**Prerequisites:**

1. Visual Studio V17.10.0 and .NET 8.0 (for .NET Aspire applications)
2. Docker

**Step 1:**

Let’s clone repository in git.

**Step 2:**

Build up Docker to prepare database:

```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build

dotnet version: 8.0
nodejs version: 18.x
npm version: 10.x.x
vite version: 4.3.4
docker compose version: 3.7
```

**Run and Debug**:

```
dotnet restore --packages .nuget
dotnet build --no-restore

dotnet new globaljson
dotnet --list-sdks
dotnet run --project "./Host/Ntech.Host.BackOffice/Ntech.Host.BackOffice.csproj"

dotnet run --project Ntech/Ntech.Migration/Ntech.Migration.csproj identity
dotnet run --project Ntech/Ntech.Migration/Ntech.Migration.csproj application

dotnet run --project Ntech/Ntech.csproj

arch -arm64 brew install llvm
sudo gem install ffi

dotnet dev-certs https --trust
```

```
dotnet workload list
dotnet workload update
dotnet workload restore
dotnet workload install aspire
cd ./Presentations
```

**Publish**

`dotnet publish -c Release`

## To Install as a global tool

```
dotnet tool install -g aspirate
dotnet tool uninstall -g aspirate
```

## Producing Manifests

```
cd ./Presentations/Ntech.Aspire.Host
aspirate generate --output-format compose

cd ./Presentations/Ntech.Aspire.Host
aspirate init
aspirate build
aspirate apply
aspirate destroy
```

**Launch APIs**

- Api documents: http://localhost:9000/swagger/index.html

```
Authentication:
{
  "ipAddress": "127.0.0.1",
  "email": "superadmin@gmail.com",
  "password": "P@ssw0rd",
  "rememberMe": true
}
```

**References**:

- https://blog.ntechdevelopers.com/
