Create Table Steel (
	Id Smallint Identity(1, 1) Primary Key,
	Name Varchar(255) Not Null Unique,
	Standard Varchar(50) Not Null,
	Strength Smallint
);