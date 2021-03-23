Create Table GeneralDataSections (
	Id Smallint Identity(1, 1) Primary Key,
	Title Varchar(255) Not Null Unique,
	ShortTitle Varchar(50) Not Null Unique,
	Print Bit Not Null,
	OrderNum Smallint Not Null,
	MultiplePoints Bit Not Null
);