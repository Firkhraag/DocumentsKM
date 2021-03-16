Create Table Specifications (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	Num Smallint Not Null,
	CreatedDate Timestamp Not Null Default Now(),
	IsCurrent Bit Not Null Default True,
	Note Varchar(255),
	Unique (MarkId, Num),
	Constraint FK_Mark_Specification Foreign Key(MarkId) References Marks(Id)
);