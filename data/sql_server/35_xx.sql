Create Table MarkGeneralDataPoints (
	Id Int Identity(1, 1) Primary Key,
	SectionId Int Not Null,
	Text Nvarchar(2000) Not Null,
	OrderNum Smallint Not Null,
	Constraint FK_MarkGeneralDataSection_MarkGeneralDataPoint Foreign Key(SectionId) References MarkGeneralDataSections(Id)
);