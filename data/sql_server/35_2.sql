Create Table GeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	SectionId Smallint Not Null,
	Text Nvarchar(2000) Not Null,
	OrderNum Smallint Not Null,
	Constraint FK_GeneralDataSection_GeneralDataPoint Foreign Key(SectionId) References GeneralDataSections(Id)
);