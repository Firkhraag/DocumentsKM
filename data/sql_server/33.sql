Create Table Profiles (
	Id Smallint Identity(1, 1) Primary Key,
	ClassId Smallint Not Null,
	Name Varchar(30) Not Null,
	Symbol Varchar(2),
	Weight Real Not Null,
	Area Real Not Null,
	TypeId Smallint Not Null,
	Unique(ClassId, Name, Symbol),
	Constraint FkClass Foreign Key(ClassId) References ProfileClasses(Id),
	Constraint FkType Foreign Key(TypeId) References ProfileTypes(Id)
);