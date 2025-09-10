-- Create database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ComplianceAnalytics')
BEGIN
    CREATE DATABASE ComplianceAnalytics;
END
GO

USE ComplianceAnalytics;
GO

-- Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(200) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role NVARCHAR(50) NOT NULL -- Admin, Manager, User
);

-- Locations table
CREATE TABLE Locations (
    LocationID INT IDENTITY(1,1) PRIMARY KEY,
    LocationName NVARCHAR(200) NOT NULL,
    Region NVARCHAR(100) NOT NULL
);

-- Tasks table
CREATE TABLE Tasks (
    TaskID INT IDENTITY(1,1) PRIMARY KEY,
    TaskName NVARCHAR(200) NOT NULL,
    LocationID INT NOT NULL,
    CompletedBy INT NULL,
    CompletionDate DATETIME NULL,
    IsCompliant BIT NOT NULL,
    WorkflowType NVARCHAR(50) NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID),
    FOREIGN KEY (CompletedBy) REFERENCES Users(UserID)
);

-- Execution Log
CREATE TABLE ProcedureExecutionLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    ProcedureName NVARCHAR(200),
    Params NVARCHAR(MAX),
    ExecutionTime INT,
    RowsReturned INT,
    ExecutedBy NVARCHAR(200),
    CreatedAt DATETIME DEFAULT GETDATE()
);
