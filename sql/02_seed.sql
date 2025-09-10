USE ComplianceAnalytics;
GO

-- Users
INSERT INTO Users (UserName, PasswordHash, Role)
VALUES 
('admin', 'hashed_admin_pw', 'Admin'),
('manager1', 'hashed_manager_pw', 'Manager'),
('user1', 'hashed_user_pw', 'User');

-- Locations
INSERT INTO Locations (LocationName, Region)
VALUES
('New Delhi Depot', 'North'),
('Mumbai Depot', 'West'),
('Chennai Depot', 'South');

-- Tasks
INSERT INTO Tasks (TaskName, LocationID, CompletedBy, CompletionDate, IsCompliant, WorkflowType)
VALUES
('Safety Check', 1, 2, GETDATE(), 1, 'Safety'),
('Equipment Audit', 2, 3, GETDATE(), 0, 'Audit'),
('Fire Drill', 3, 3, GETDATE(), 1, 'Safety'),
('Process Review', 1, NULL, NULL, 0, 'Audit');
