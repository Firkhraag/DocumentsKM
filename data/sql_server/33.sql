Create Table Profiles (
	Id Int Identity(1, 1) Primary Key,
	ClassId Smallint Not Null,
	Name Varchar(30) Not Null,
	Symbol Varchar(2),
	Weight Real Not Null,
	Area Real Not Null,
	TypeId Smallint Not Null,
	Unique(ClassId, Name, Symbol),
	Constraint FK_ProfileClass_Profile Foreign Key(ClassId) References ProfileClasses(Id),
	Constraint FK_ProfileType_Profile Foreign Key(TypeId) References ProfileTypes(Id)
);