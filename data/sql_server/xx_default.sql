Create Table DefaultValues (
	UserId Smallint Identity(1, 1) Primary Key,
	DepartmentId Smallint,
	CreatorId Smallint,
	InspectorId Smallint,
	NormContrId Smallint,
	Constraint FkUser Foreign Key(UserId) References Users(Id),
	Constraint FkDepartment Foreign Key(DepartmentId) References Departments(Id),
	Constraint FkCreator Foreign Key(CreatorId) References Employees(Id),
	Constraint FkInspector Foreign Key(InspectorId) References Employees(Id),
	Constraint FkNormContr Foreign Key(NormContrId) References Employees(Id)
);