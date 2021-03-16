Create Table Primer (
	Id Smallint Identity(1, 1) Primary Key,
	GroupNum Smallint Not Null,
	Name Varchar(255) Not Null,
	CanBePrimed Bit Not Null,
	Priority Smallint Not Null Default 0,
	Unique(GroupNum, Name)
);