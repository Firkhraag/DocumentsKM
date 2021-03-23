Create Table ConstructionElements (
	Id Int Identity(1, 1) Primary Key,
	ConstructionId Int Not Null,
	ProfileClassId Smallint Not Null,
	ProfileName Varchar(30) Not Null,
	Symbol Varchar(2) Not Null,
	Weight Real Not Null,
	SurfaceArea Real Not Null,
	ProfileTypeId Smallint Not Null,
	SteelId Smallint Not Null,
	Length Real Not Null,
	Status Smallint Not Null,
	Constraint FkConstruction Foreign Key(ConstructionId) References Constructions(Id),
	Constraint FkProfileClass Foreign Key(ProfileClassId) References ProfileClasses(Id),
	Constraint FkProfileType Foreign Key(ProfileTypeId) References ProfileTypes(Id),
	Constraint FkSteel Foreign Key(SteelId) References Steel(Id)
);