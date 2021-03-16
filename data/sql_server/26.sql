Create Table Employees (
	Id Int Identity(1, 1) Primary Key,
	PositionId Smallint Not Null,
	Fullname Varchar(255) Not Null,
	DepartmentId Smallint Not Null,
	Constraint FkPosition Foreign Key(PositionId) References Positions(Id),
	Constraint FkDepartment Foreign Key(DepartmentId) References Departments(Id)
);