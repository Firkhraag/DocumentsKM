CREATE TABLE bolt_diameters (
	id smallserial PRIMARY KEY,
	diameter smallint NOT NULL,
	nut_weight real NOT NULL,
	washer_steel varchar(50) NOT NULL,
	washer_weight real NOT null,
	washer_thickness smallint NOT NULL,
	bolt_tech_spec varchar(50) NOT NULL,
	strength_class varchar(50) NOT NULL,
	nut_tech_spec varchar(50) NOT NULL,
	washer_tech_spec varchar(50) NOT NULL,
	UNIQUE(
		diameter, nut_weight, washer_steel, washer_weight,
		washer_thickness, bolt_tech_spec, strength_class,
		nut_tech_spec, washer_tech_spec
	)
);