Create Table MarkOperatingConditions (
	MarkId Int Primary Key,
	SafetyCoeff Real,
	Temperature Smallint,
	OperatingAreaId Smallint,
	GasGroupId Smallint,
	EnvAggressivenessId Smallint,
	ConstructionMaterialId Smallint,
	PaintworkTypeId Smallint,
	HighTensileBoltsTypeId Smallint,
	Constraint FkMark Foreign Key(MarkId) References Marks(Id),
	Constraint FkOperatingArea Foreign Key(OperatingAreaId) References OperatingAreas(Id),
	Constraint FkGasGroup Foreign Key(GasGroupId) References GasGroups(Id),
	Constraint FkEnvAggressiveness Foreign Key(EnvAggressivenessId) References EnvAggressiveness(Id),
	Constraint FkConstructionMaterial Foreign Key(ConstructionMaterialId) References ConstructionMaterials(Id),
	Constraint FkPaintworkType Foreign Key(PaintworkTypeId) References PaintworkTypes(Id),
	Constraint FkHighTensileBoltsType Foreign Key(HighTensileBoltsTypeId) References HighTensileBoltsTypes(Id)
);