Create Table DocTypes (
	Id Smallint Identity(1, 1) Primary Key,
	Code Varchar(4) Not Null Unique,
	Name Varchar(100) Not Null Unique
);