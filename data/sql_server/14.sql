Create Table MarkLinkedDocs (
	Id Smallint Identity(1, 1) Primary Key,
	MarkId Int,
	LinkedDocId Smallint,
	Note Varchar(50),
	Unique(MarkId, LinkedDocId),
	Constraint FK_Mark_MarkLinkedDoc Foreign Key(MarkId) References Marks(Id),
	Constraint FK_LinkedDoc_MarkLinkedDoc Foreign Key(LinkedDocId) References LinkedDocs(Id)
);