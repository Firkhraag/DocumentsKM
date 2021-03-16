Create Table Nodes (
	Id Smallint Identity(1, 1) Primary Key,
	Code Varchar(10) Not Null,
	Name Varchar(255) Not Null,
	ProjectId Smallint Not Null,
	ChiefEngineerId Smallint Not Null,
	Constraint FkProject Foreign Key(ProjectId) References Projects(Id),
	Constraint FkEmployee Foreign Key(ChiefEngineerId) References Employees(Id)
);