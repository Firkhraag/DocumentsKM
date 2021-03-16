Create Table EstimateTask (
	MarkId Int Primary Key,
	TaskText Varchar Not Null,
	AdditionalText Varchar,
	ApprovalEmployeeId Smallint,
	Constraint FkMark Foreign Key(MarkId) References Marks(Id),
	Constraint FkEmployee Foreign Key(ApprovalEmployeeId) References Employees(Id)
);