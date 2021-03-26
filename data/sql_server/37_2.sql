Create Table GeneralDataSections (
	Id Smallint Identity(1, 1) Primary Key,
	UserId Smallint Not Null,
	Name Varchar(255) Not Null,
	OrderNum Smallint Not Null,
	Unique(UserId, Name)
);