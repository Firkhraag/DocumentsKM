Create Table CorrProtVariants (
	Id Smallint Identity(1, 1) Primary Key,
	OperatingAreaId Smallint Not Null,
	GasGroupId Smallint Not Null,
	EnvAggressivenessId Smallint Not Null,
	ConstructionMaterialId Smallint Not Null,
	PaintworkTypeId Smallint,
	PaintworkGroup Smallint,
	PaintworkFastnessId Smallint,
	PaintworkNumOfLayers Smallint,
	PaintworkPrimerThickness Smallint,
	PrimerNumOfLayers Smallint,
	CleaningDegreeId Smallint,
	Status Smallint Not Null,
	Unique(
		OperatingAreaId, GasGroupId, EnvAggressivenessId,
		ConstructionMaterialId, PaintworkTypeId, PaintworkGroup
	),
	Constraint FkOperatingArea Foreign Key(OperatingAreaId) References OperatingAreas(Id),
	Constraint FkGasGroup Foreign Key(GasGroupId) References GasGroups(Id),
	Constraint FkEnvAggressiveness Foreign Key(EnvAggressivenessId) References EnvAggressiveness(Id),
	Constraint FkConstructionMaterial Foreign Key(ConstructionMaterialId) References ConstructionMaterials(Id),
	Constraint FkPaintworkType Foreign Key(PaintworkTypeId) References PaintworkTypes(Id),
	Constraint FkPaintworkFastness Foreign Key(PaintworkFastnessId) References PaintworkFastness(Id),
	Constraint FkCleaningDegree Foreign Key(CleaningDegreeId) References CorrProtCleaningDegrees(Id)
);