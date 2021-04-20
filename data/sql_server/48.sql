Create Table ConstructionElements (
	Id Int Identity(1, 1) Primary Key,
	ConstructionId Int Not Null,
	ProfileId Int Not Null,
	SteelId Smallint Not Null,
	Length Real Not Null,
	ArithmeticExpression Varchar(100),
	Constraint FK_Construction_ConstructionElement Foreign Key(ConstructionId) References Constructions(Id),
	Constraint FK_Profile_ConstructionElement Foreign Key(ProfileId) References Profiles(Id),
	Constraint FK_Steel_ConstructionElement Foreign Key(SteelId) References Steel(Id)
);