Create Table DefaultValues (
	UserId Smallint Identity(1, 1) Primary Key,
	DepartmentId Smallint,
	CreatorId Int,
	InspectorId Int,
	NormContrId Int,
	Constraint FK_User_DefaultValues Foreign Key(UserId) References Users(Id),
	Constraint FK_Department_DefaultValues Foreign Key(DepartmentId) References Departments(Id),
	Constraint FK_Creator_DefaultValues Foreign Key(CreatorId) References Employees(Id),
	Constraint FK_Inspector_DefaultValues Foreign Key(InspectorId) References Employees(Id),
	Constraint FK_NormContr_DefaultValues Foreign Key(NormContrId) References Employees(Id)
);