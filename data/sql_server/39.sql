Create Table LinkedDocs (
	Id Smallint Identity(1, 1) Primary Key,
	Code Varchar(4) Not Null Unique,
	TypeId Smallint Not Null,
	Designation Varchar(40) Not Null,
	Name Varchar(255) Not Null Unique,
	Constraint FkLinkedDocType Foreign Key(TypeId) References LinkedDocTypes(Id)
);