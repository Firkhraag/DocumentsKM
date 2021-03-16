Create Table ConstructionElements (
	Id Int Identity(1, 1) Primary Key,
	ConstructionId Int Not Null,
	ProfileId Int Not Null,
	SteelId Smallint Not Null,
	Length Real Not Null,
	Constraint FkConstruction Foreign Key(ConstructionId) References Constructions(Id),
	Constraint FkProfile Foreign Key(ProfileId) References Profiles(Id),
	Constraint FkSteel Foreign Key(SteelId) References Steel(Id)
);