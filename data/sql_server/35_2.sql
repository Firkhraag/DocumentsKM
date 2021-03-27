Create Table GeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	SectionId Smallint Not Null,
	Text Nvarchar(4000) Not Null,
	OrderNum Smallint Not Null,
	Unique(SectionId, Text),
	Constraint FK_GeneralDataSection_GeneralDataPoint Foreign Key(SectionId) References GeneralDataSections(Id)
);