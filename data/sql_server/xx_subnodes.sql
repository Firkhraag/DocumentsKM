Create Table Subnodes (
	Id Smallint Identity(1, 1) Primary Key,
	NodeId Smallint Not Null,
	Code Varchar(10) Not Null,
	Name Varchar(255) Not Null,
	Constraint FK_Node_Subnode Foreign Key(NodeId) References Nodes(Id)
);