Create Table Nodes (
	Id Int Primary Key,
	Code Varchar(10) Not Null,
	Name Varchar(255) Not Null,
	ProjectId Int Not Null,
	ChiefEngineerId Int Not Null,
	Constraint FK_Project_Node Foreign Key(ProjectId) References Projects(Id),
	Constraint FK_Employee_Node Foreign Key(ChiefEngineerId) References Employees(Id)
);