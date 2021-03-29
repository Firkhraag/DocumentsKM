Create Table Nodes (
	Id Int Primary Key,
	Code Varchar(10) Not Null,
	Name Nvarchar(255),
	ProjectId Int Not Null,
	ChiefEngineer Varchar(255),
	Unique(ProjectId, Code),
	Constraint FK_Project_Node Foreign Key(ProjectId) References Projects(Id)
);