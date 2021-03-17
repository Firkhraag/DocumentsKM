Create Table EstimateTask (
	MarkId Int Primary Key,
	TaskText Varchar Not Null,
	AdditionalText Varchar,
	ApprovalEmployeeId Int,
	Constraint FK_Mark_EstimateTask Foreign Key(MarkId) References Marks(Id),
	Constraint FK_Employee_EstimateTask Foreign Key(ApprovalEmployeeId) References Employees(Id)
);