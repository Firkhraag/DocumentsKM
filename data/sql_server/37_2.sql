Create Table GeneralDataSections (
	Id Smallint Identity(1, 1) Primary Key,
	Name Varchar(255) Not Null Unique,
	OrderNum Smallint Not Null Unique
);