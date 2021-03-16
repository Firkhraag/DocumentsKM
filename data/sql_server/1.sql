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
	Constraint FK_OperatingArea_CorrProtVariant Foreign Key(OperatingAreaId) References OperatingAreas(Id),
	Constraint FK_GasGroup_CorrProtVariant Foreign Key(GasGroupId) References GasGroups(Id),
	Constraint FK_EnvAggressiveness_CorrProtVariant Foreign Key(EnvAggressivenessId) References EnvAggressiveness(Id),
	Constraint FK_ConstructionMaterial_CorrProtVariant Foreign Key(ConstructionMaterialId) References ConstructionMaterials(Id),
	Constraint FK_PaintworkType_CorrProtVariant Foreign Key(PaintworkTypeId) References PaintworkTypes(Id),
	Constraint FK_PaintworkFastness_CorrProtVariant Foreign Key(PaintworkFastnessId) References PaintworkFastness(Id),
	Constraint FK_CleaningDegree_CorrProtVariant Foreign Key(CleaningDegreeId) References CorrProtCleaningDegrees(Id)
);