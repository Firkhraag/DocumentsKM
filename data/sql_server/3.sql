Create Table Specifications (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	Num Smallint Not Null,
	CreatedDate Timestamp Not Null Default Now(),
	IsCurrent Boolean Not Null Default True,
	Note Varchar(255),
	Unique (MarkId, Num),
	Constraint FkMark Foreign Key(MarkId) References Marks(Id)
);