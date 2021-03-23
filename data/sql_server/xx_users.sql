Create Table Users (
	Id Smallint Identity(1, 1) Primary Key,
	Login Varchar(255) Not Null,
	Password Varchar(255) Not Null,
	EmployeeId Int Not Null,
	Constraint FK_Employee_User Foreign Key(EmployeeId) References Employees(Id)
);