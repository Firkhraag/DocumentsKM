CREATE TABLE bolt_lengths (
	id smallserial PRIMARY KEY,
	bolt_diameter_id smallint NOT NULL,
	bolt_len smallint NOT NULL,
	screw_len smallint NOT NULL,
	bolt_weight real NOT NULL,
	UNIQUE(
		bolt_diameter_id, bolt_len,
		screw_len, bolt_weight
	)
);