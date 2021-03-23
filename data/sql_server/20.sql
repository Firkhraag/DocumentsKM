Create Table BoltDiameters (
	Id Smallint Identity(1, 1) Primary Key,
	Diameter Smallint Not Null,
	NutWeight Real Not Null,
	WasherSteel Varchar(50) Not Null,
	WasherWeight Real Not Null,
	WasherThickness Smallint Not Null,
	BoltTechSpec Varchar(50) Not Null,
	StrengthClass Varchar(50) Not Null,
	NutTechSpec Varchar(50) Not Null,
	WasherTechSpec Varchar(50) Not Null,
	Unique(
		Diameter, NutWeight, WasherSteel, WasherWeight,
		WasherThickness, BoltTechSpec, StrengthClass,
		NutTechSpec, WasherTechSpec
	)
);