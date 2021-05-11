Create Table MarkGeneralDataSections (
	Id Int Identity(1, 1) Primary Key,
	MarkId int Not Null,
	Name Varchar(255) Not Null,
	OrderNum Smallint Not Null,
	Unique(MarkId, Name),
	Constraint GeneralDataSection Foreign Key(MarkId) References Marks(Id)
);