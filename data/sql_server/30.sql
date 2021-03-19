Create Table Departments (
	Id Smallint Identity(1, 1) Primary Key,
	Code Varchar(6) Not Null Unique,
	Name Varchar(255) Not Null,
	ShortName Varchar(50) Not Null,
    IsActive Bit Not Null
);