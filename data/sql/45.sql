CREATE TABLE construction_bolts (
	id smallserial PRIMARY KEY,
	construction_id smallint NOT NULL,
	diameter_id smallint NOT NULL,
	packet smallint NOT NULL,
	num smallint NOT NULL,
	nut_num smallint NOT NULL,
	washer_num smallint NOT NULL,
	UNIQUE (construction_id, diameter_id),
	CONSTRAINT fk_construction FOREIGN KEY(construction_id) REFERENCES constructions(id),
	CONSTRAINT fk_diameter FOREIGN KEY(diameter_id) REFERENCES bolt_diameters(id)
);