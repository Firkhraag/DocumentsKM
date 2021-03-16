Create Table BoltLengths (
	Id Smallint Identity(1, 1) Primary Key,
	DiameterId Smallint Not Null,
	Length Smallint Not Null,
	ScrewLength Smallint Not Null,
	Weight Real Not Null,
	Unique(DiameterId, Length),
	Constraint Diameter Foreign Key(DiameterId) References BoltDiameters(Id)
);