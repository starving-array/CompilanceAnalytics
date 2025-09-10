# 📊 Compliance Analytics – Setup Guide  

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)  
![Docker](https://img.shields.io/badge/Docker-Desktop-2496ED?logo=docker&logoColor=white)  
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver&logoColor=white)  
![Redis](https://img.shields.io/badge/Redis-7-DC382D?logo=redis&logoColor=white)  

A full-stack .NET 8 project with **SQL Server 2022**, **Redis cache**, and **JWT authentication**.  
Easily spin up the environment with Docker and start running compliance analytics queries.

---

## 🚀 What You’ll Get
- ✅ SQL Server 2022 running inside Docker  
- ✅ Database initialized with **schema**, **seed data**, and **stored procedure**  
- ✅ Optional **Redis cache** (via Docker)  
- ✅ .NET 8 API connected to the DB  
- ✅ Ability to **register/login users** and query analytics with JWT authentication  

---

## 📋 Prerequisites
Make sure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop) (with Docker Compose)  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- (Optional) [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio) or SSMS for DB inspection  

Check installed SDK:

dotnet --list-sdks
It should show 8.x.

## 📂 Project Layout
```
ComplianceAnalytics/
├── docker-compose.yml          # Docker config
├── sql/                        # Auto-run DB scripts
│   ├── 01_init.sql             # Schema
│   ├── 02_seed.sql             # Sample data
│   └── 03_procedure.sql        # Stored procedure
├── ComplianceAnalytics.sln
├── ComplianceAnalytics.API/    
├── ComplianceAnalytics.Application/
├── ComplianceAnalytics.Infrastructure/
├── ComplianceAnalytics.Domain/
└── README.md

```

## ⚙️ Configuration
```
Already configured in ComplianceAnalytics.API/appsettings.json:

json

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=ComplianceAnalytics;User Id=sa;Password=DOTnet@1234;TrustServerCertificate=True"
},
"Jwt": {
  "Key": "This_is_a_very_long_and_secure_secret_key_123!",
  "Issuer": "ComplianceAPI",
  "Audience": "ComplianceUsers",
  "ExpireMinutes": 60
}
🐳 Docker Setup
Your docker-compose.yml (in project root):

yaml

version: "3.9"

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: compliance_sql
    ports:
      - "1433:1433"
    environment:
      SA_PASSWORD: ${SA_PASSWORD:-DOTnet@1234}
      ACCEPT_EULA: "Y"
    volumes:
      - sql_data:/var/opt/mssql
      - ./sql:/docker-entrypoint-initdb.d
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    container_name: compliance_redis
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    restart: unless-stopped
    # Comment out if Redis already installed locally

volumes:
  sql_data:
  redis_data:
  ```
## 🗄️ SQL Scripts
```
01_init.sql → Creates database, tables, and log table

02_seed.sql → Inserts sample locations, tasks, and placeholder users

03_procedure.sql → Defines usp_GetComplianceAnalytics stored procedure
```

⚠️ Note: Users in seed script use placeholder hashes.
👉 Always create users via the API to ensure proper password hashing.

### ▶️ Run Everything

1️⃣ Start containers
```
docker-compose up -d
```
2️⃣ Watch SQL logs until DB is ready
```
docker-compose logs -f sqlserver
```
3️⃣ Connect with Azure Data Studio
```
Server: localhost,1433

User: sa

Password: DOTnet@1234

Database: ComplianceAnalytics
```
4️⃣ Verify DB
```
SELECT TOP 5 * FROM Users;
EXEC usp_GetComplianceAnalytics;
```
🌐 Run the API

From project root:
```
dotnet restore
dotnet build
dotnet run --project ComplianceAnalytics.API
```

### Default URLs:
```
Swagger → https://localhost:5001/swagger

API → http://localhost:5000
```
## 👤 Register & Login (API)
#### Register a user
```
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"userName": "admin", "password": "Admin@123", "role": "Admin"}'
  ```
#### Login to get JWT
```
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"userName": "admin", "password": "Admin@123"}'
  ```
#### Use JWT for secured endpoints
```
curl -H "Authorization: Bearer <TOKEN>" \
  "https://localhost:5001/api/analytics/compliance?Region=North"
  ```

#### 🔑 Redis Tips
```
Runs on localhost:6379
```

#### Clear cache in dev:

```
redis-cli -p 6379 FLUSHALL
```

## 🛠️ Useful Commands
#### Stop containers:

```
docker-compose down
```
#### Stop & wipe volumes (DB reset):

```
docker-compose down -v
```
#### Re-run SQL script manually:

```
docker exec -it compliance_sql /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P 'DOTnet@1234' \
  -i /docker-entrypoint-initdb.d/03_procedure.sql
  ```