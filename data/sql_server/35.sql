Create Table GeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	SectionId Smallint Not Null,
	Title Varchar(80) Not Null,
	Text Nvarchar(2000),
	DoNotCut Bit Not Null,
	FixedPoint Smallint Not Null,
	OrderNum Smallint Not Null,
	TableName Varchar(10),
	Constraint FK_GeneralDataSection_GeneralDataPoint Foreign Key(SectionId) References GeneralDataSections(Id)
);