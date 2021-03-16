Create Table GeneralGuidancePoints (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	Point Smallint Not Null,
	Text Varchar,
	Include Boolean Not Null,
	Original Boolean Not Null,
	DoNotCut Boolean Not Null,
	PointType Smallint Not Null Default 0,
	Unique(MarkId, Point)
);