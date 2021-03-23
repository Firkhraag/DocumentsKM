Create Table AdditionalWork (
	Id Int Identity(1, 1) Primary Key,
	MarkId Int Not Null,
	EmployeeId Int Not Null,
	Valuation Smallint Not Null,
	MetalOrder Smallint Not Null,
	Unique(MarkId, EmployeeId)
);