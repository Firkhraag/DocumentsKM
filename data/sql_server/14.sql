Create Table MarkLinkedDocs (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	LinkedDocId Smallint Not null,
	Note Varchar(50),
	Unique(MarkId, LinkedDocId),
	Constraint FK_Mark_MarkLinkedDoc Foreign Key(MarkId) References Marks(Id),
	Constraint FK_LinkedDoc_MarkLinkedDoc Foreign Key(LinkedDocId) References LinkedDocs(Id)
);