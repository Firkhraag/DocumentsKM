Create Table MarkGeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	MarkId Smallint Not Null,
	SectionId Smallint Not Null,
	Text Varchar Not Null,
	OrderNum Smallint Not Null,
	Unique(MarkId, SectionId, Text),
	Constraint FK_Mark_MarkGeneralDataPoint Foreign Key(MarkId) References Marks(Id),
	Constraint FK_GeneralDataSection_MarkGeneralDataPoint Foreign Key(SectionId) References GeneralDataSections(Id)
);