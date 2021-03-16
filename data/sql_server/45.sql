Create Table ConstructionBolts (
	Id Smallint Identity(1, 1) Primary Key,
	ConstructionId Smallint Not Null,
	DiameterId Smallint Not Null,
	Packet Smallint Not Null,
	Num Smallint Not Null,
	NutNum Smallint Not Null,
	WasherNum Smallint Not Null,
	Unique (ConstructionId, DiameterId),
	Constraint FkConstruction Foreign Key(ConstructionId) References Constructions(Id),
	Constraint FkDiameter Foreign Key(DiameterId) References BoltDiameters(Id)
);