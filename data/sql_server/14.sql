Create Table MarkLinkedDocs (
	Id Smallint Identity(1, 1) Primary Key,
	MarkId Int,
	LinkedDocId Smallint,
	Note Varchar(50),
	Unique(MarkId, LinkedDocId),
	Constraint FkMark Foreign Key(MarkId) References Marks(Id),
	Constraint FkLinkedDoc Foreign Key(LinkedDocId) References LinkedDocs(Id)
);