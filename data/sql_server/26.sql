Create Table Employees (
	Id Int Primary Key,
	PositionId Smallint Not Null,
	Fullname Varchar(255) Not Null,
	Name Varchar(80) Not Null,
	DepartmentId Smallint Not Null,
	Constraint FK_Position_Employee Foreign Key(PositionId) References Positions(Id),
	Constraint FK_Department_Employee Foreign Key(DepartmentId) References Departments(Id)
);