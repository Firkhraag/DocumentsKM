Create Table MarkDesignations (
	Id Smallint Identity(1, 1) Primary Key,
	Designation Varchar(4) Not Null Unique,
	Name Varchar(50) Not Null Unique
);