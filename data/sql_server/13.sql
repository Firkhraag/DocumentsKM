Create Table MarkApprovals (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int,
	EmployeeId Int,
	Unique (MarkId, EmployeeId),
	Constraint FkMark Foreign Key(MarkId) References Marks(Id),
	Constraint FkEmployee Foreign Key(EmployeeId) References Employees(Id)
);