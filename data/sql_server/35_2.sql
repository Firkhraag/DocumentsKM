Create Table GeneralDataPoints (
	Id Smallint Identity(1, 1) Primary Key,
	UserId Smallint Not Null,
	SectionId Smallint Not Null,
	Text Varchar Not Null,
	OrderNum Smallint Not Null,
	Unique(UserId, SectionId, Text),
	Constraint FkUser Foreign Key(UserId) References Users(Id),
	Constraint FkSection Foreign Key(SectionId) References GeneralDataSections(Id)
);