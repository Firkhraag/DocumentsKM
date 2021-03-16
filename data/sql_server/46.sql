Create Table Constructions (
	Id Int Identity(1, 1) Primary Key,
	SpecificationId Int Not Null,
	Name Varchar(255) Not Null,
	TypeId Smallint Not Null,
	SubtypeId Smallint,
	Valuation Varchar(10),
	StandardAlbumCode Varchar(20),
	NumOfStandardConstructions Smallint Not Null,
	HasEdgeBlunting Boolean Not Null,
	HasDynamicLoad Boolean Not Null,
	HasFlangedConnections Boolean Not Null,
	WeldingControlId Smallint Not Null,
	PaintworkCoeff Real Not Null,
	Unique (SpecificationId, Name, PaintworkCoeff),
	Constraint FkSpecification Foreign Key(SpecificationId) References Specifications(Id),
	Constraint FkType Foreign Key(TypeId) References ConstructionTypes(Id),
	Constraint FkSubtype Foreign Key(SubtypeId) References ConstructionSubtypes(Id),
	Constraint FkWeldingControl Foreign Key(WeldingControlId) References WeldingControl(Id)
);