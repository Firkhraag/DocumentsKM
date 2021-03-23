Create Table GeneralGuidancePoints (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	Point Smallint Not Null,
	Text Varchar,
	Include Bit Not Null,
	Original Bit Not Null,
	DoNotCut Bit Not Null,
	PointType Smallint Not Null Default 0,
	Unique(MarkId, Point)
);