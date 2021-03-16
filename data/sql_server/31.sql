Create Table OzPaintings (
	Id Smallint Identity(1, 1) Primary Key,
	Name Varchar(100) Unique Not Null,
	ShortName Varchar(30) Unique Not Null,
	Include Bit Not Null
);