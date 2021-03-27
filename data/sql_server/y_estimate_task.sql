Create Table EstimateTask (
	MarkId Int Primary Key,
	TaskText Nvarchar(1000) Not Null,
	AdditionalText Nvarchar(4000),
	ApprovalEmployeeId Int,
	Constraint FK_Mark_EstimateTask Foreign Key(MarkId) References Marks(Id),
	Constraint FK_Employee_EstimateTask Foreign Key(ApprovalEmployeeId) References Employees(Id)
);