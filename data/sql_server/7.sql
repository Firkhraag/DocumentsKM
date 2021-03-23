Create Table Marks (
	Id Int Identity(1, 1) Primary Key,
	Code Varchar(40) Not Null,
	Name Varchar(255) Not Null,
	SubnodeId Int Not Null,
	DepartmentId Smallint Not Null,
	Signed1Id Int,
	Signed2Id Int,
	ChiefSpecialistId Int,
	GroupLeaderId Int,
	NormContrId Int,
	IssueDate Date,
	NumOfVolumes Smallint,
	EditedDate Date,
	PaintworkType Varchar(4),
	Note Varchar(255),
	FireHazardCategoryId Smallint,
	PTransport Bit,
	PSite Bit,
	Unique (Code, SubnodeId),
	Constraint FK_Subnode_Mark Foreign Key(SubnodeId) References Subnodes(Id),
	Constraint FK_ChiefSpecialist_Mark Foreign Key(ChiefSpecialistId) References Employees(Id),
	Constraint FK_GroupLeader_Mark Foreign Key(GroupLeaderId) References Employees(Id),
	Constraint FK_NormContr_Mark Foreign Key(NormContrId) References Employees(Id),
	Constraint FK_FireHazardCategory_Mark Foreign Key(FireHazardCategoryId) References FireHazardCategories(Id)
);