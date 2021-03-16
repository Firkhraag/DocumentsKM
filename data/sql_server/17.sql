Create Table CorrProtCoatings (
	Id Smallint Identity(1, 1) Primary Key,
	PaintworkTypeId Smallint Not Null,
	PaintworkGroup Smallint Not Null,
	PaintworkFastnessId Smallint Not Null,
	Name Varchar(255) Not Null,
	PaintworkNumOfLayers Smallint,
	PrimerGroup Smallint Not Null,
	CanBePainted Bit Not Null,
	Priority Smallint Not Null,
	Unique(
		PaintworkTypeId, PaintworkGroup, PaintworkFastnessId, Name
	),
	Constraint FK_PaintworkType_CorrProtCoating Foreign Key(PaintworkTypeId) References PaintworkTypes(Id),
	Constraint FK_PaintworkFastness_CorrProtCoating Foreign Key(PaintworkFastnessId) References PaintworkFastness(Id)
);