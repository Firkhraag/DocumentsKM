Create Table GeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	UserId Smallint Not Null,
	SectionId Smallint Not Null,
	Text Varchar(1000) Not Null,
	OrderNum Smallint Not Null,
	Unique(UserId, SectionId, Text),
	Constraint FK_User_GeneralDataPoint Foreign Key(UserId) References Users(Id),
	Constraint FK_GeneralDataSection_GeneralDataPoint Foreign Key(SectionId) References GeneralDataSections(Id)
);