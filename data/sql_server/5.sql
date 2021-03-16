Create Table FireHazardCategories (
	Id Smallint Identity(1, 1) Primary Key,
	Category Varchar(1) Not Null Unique,
	Name Varchar(30) Not Null,
	Description Varchar(255)
);