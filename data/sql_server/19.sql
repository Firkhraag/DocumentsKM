Create Table CorrProtMethods (
	Id Smallint Identity(1, 1) Primary Key,
	EnvAggressivenessId Smallint Not Null,
	ConstructionMaterialId Smallint Not Null,
	Name Varchar(255) Not Null,
	Status Smallint Not Null,
	Unique(
		EnvAggressivenessId, ConstructionMaterialId, Name
	),
	Constraint FkEnvAggressiveness Foreign Key(EnvAggressivenessId) References EnvAggressiveness(Id),
	Constraint FkConstructionMaterial Foreign Key(ConstructionMaterialId) References ConstructionMaterials(Id)
);