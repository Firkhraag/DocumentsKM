Create Table OrganizationName (
	Model Nvarchar(255) Not Null,
	Name Nvarchar(255) Not Null,
	ShortName Nvarchar(255) Not Null,
    Primary Key(Model, Name, ShortName)
);