Create Table Subnodes (
	Id Int Primary Key,
	NodeId Int Not Null,
	Code Varchar(10) Not Null,
	Name Varchar(255) Not Null,
	Constraint FK_Node_Subnode Foreign Key(NodeId) References Nodes(Id)
);