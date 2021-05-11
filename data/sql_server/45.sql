Create Table ConstructionBolts (
	Id Int Identity(1, 1) Primary Key,
	ConstructionId Int Not Null,
	DiameterId Smallint Not Null,
	Packet Smallint Not Null,
	Num Smallint Not Null,
	NutNum Smallint Not Null,
	WasherNum Smallint Not Null,
	Unique (ConstructionId, DiameterId),
	Constraint FK_Construction_ConstructionBolt Foreign Key(ConstructionId) References Constructions(Id),
	Constraint FK_BoltDiameter_ConstructionBolt Foreign Key(DiameterId) References BoltDiameters(Id)
);