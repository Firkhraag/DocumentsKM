Create Table AttachedDocs (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	Designation Varchar(100) Not Null,
	Name Varchar(200) Not Null,
	Note Varchar(50),
	Unique(MarkId, Designation)
);