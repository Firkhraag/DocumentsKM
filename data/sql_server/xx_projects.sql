Create Table Projects (
	Id Int Identity(1, 1) Primary Key,
	BaseSeries Varchar(20) Not Null Unique,
	Name Varchar(255) Not Null
);