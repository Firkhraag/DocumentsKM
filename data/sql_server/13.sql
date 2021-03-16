Create Table MarkApprovals (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int,
	EmployeeId Int,
	Unique (MarkId, EmployeeId),
	Constraint FK_Mark_MarkApproval Foreign Key(MarkId) References Marks(Id),
	Constraint FK_Employee_MarkApproval Foreign Key(EmployeeId) References Employees(Id)
);