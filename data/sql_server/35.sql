Create Table GeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	SectionId Smallint Not Null,
	Title Varchar(80) Not Null,
	Text Varchar,
	DoNotCut Boolean Not Null,
	FixedPoint Smallint Not Null,
	OrderNum Smallint Not Null,
	TableName Varchar(10),
	Unique(SectionId, OrderNum),
	Constraint FkSection Foreign Key(SectionId) References GeneralDataSections(Id)
);