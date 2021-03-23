Create Table ConstructionSubtypes (
	Id Smallint Identity(1, 1) Primary Key,
	TypeId Smallint Not Null,
	Name Varchar(255) Not Null,
	Valuation Varchar(10) Not Null,
	Unique(TypeId, Name, Valuation),
	Constraint FK_ConstructionType_ConstructionSubtype Foreign Key(TypeId) References ConstructionTypes(Id)
);