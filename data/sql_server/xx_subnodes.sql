Create Table Subnodes (
	Id Int Primary Key,
	NodeId Int Not Null,
	Code Varchar(10) Not Null,
	Name Nvarchar(255),
	Unique(NodeId, Code),
	Constraint FK_Node_Subnode Foreign Key(NodeId) References Nodes(Id)
);