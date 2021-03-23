Create Table CorrProtMethods (
	Id Smallint Identity(1, 1) Primary Key,
	EnvAggressivenessId Smallint Not Null,
	ConstructionMaterialId Smallint Not Null,
	Name Varchar(255) Not Null,
	Status Smallint Not Null,
	Unique(
		EnvAggressivenessId, ConstructionMaterialId, Name
	),
	Constraint FK_EnvAggressiveness_CorrProtMethod Foreign Key(EnvAggressivenessId) References EnvAggressiveness(Id),
	Constraint FK_ConstructionMaterial_CorrProtMethod Foreign Key(ConstructionMaterialId) References ConstructionMaterials(Id)
);