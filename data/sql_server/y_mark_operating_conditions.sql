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
	Constraint FK_Mark_MarkOperatingConditions Foreign Key(MarkId) References Marks(Id),
	Constraint FK_OperatingArea_MarkOperatingConditions Foreign Key(OperatingAreaId) References OperatingAreas(Id),
	Constraint FK_GasGroup_MarkOperatingConditions Foreign Key(GasGroupId) References GasGroups(Id),
	Constraint FK_EnvAggressiveness_MarkOperatingConditions Foreign Key(EnvAggressivenessId) References EnvAggressiveness(Id),
	Constraint FK_ConstructionMaterial_MarkOperatingConditions Foreign Key(ConstructionMaterialId) References ConstructionMaterials(Id),
	Constraint FK_PaintworkType_MarkOperatingConditions Foreign Key(PaintworkTypeId) References PaintworkTypes(Id),
	Constraint FK_HighTensileBoltsType_MarkOperatingConditions Foreign Key(HighTensileBoltsTypeId) References HighTensileBoltsTypes(Id)
);