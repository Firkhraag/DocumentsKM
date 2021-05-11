Create Table StandardConstructions (
	Id Int Identity(1, 1) Primary Key,
	SpecificationId Int Not Null,
	Name Varchar(255) Not Null,
	Num Smallint Not Null,
	Sheet Varchar(255),
	Weight Real Not Null,
	Unique (SpecificationId, Name),
	Constraint FK_Specification_StandardConstruction Foreign Key(SpecificationId) References Specifications(Id)
);