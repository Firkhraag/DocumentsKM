Create Table OzPaintingThickness (
	Id Smallint Identity(1, 1) Primary Key,
	Lim Smallint Not Null,
	MetalThickness Real Not Null,
	OzThickness Real Not Null,
	OzExpenditure Real Not Null,
	Unique(Lim, MetalThickness)
);