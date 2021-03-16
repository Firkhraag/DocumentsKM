Create Table FragmentSelection (
	Id Smallint Identity(1, 1) Primary Key,
	ConditionNum Smallint Not Null,
	Selection Smallint Not Null,
	Note Varchar(50),
	ExcludePoint Smallint Not Null,
	Unique(ConditionNum, Selection, ExcludePoint)
);