Create Table ProfileClasses (
	Id Smallint Identity(1, 1) Primary Key,
	Name Varchar(255) Unique Not Null,
	Note Varchar(255)
);