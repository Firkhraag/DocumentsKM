CREATE TABLE bolt_lengths (
	id smallint identity(1, 1) PRIMARY KEY,
	diameter_id smallint NOT NULL,
	length smallint NOT NULL,
	screw_length smallint NOT NULL,
	weight real NOT NULL,
	UNIQUE(diameter_id, length),
	CONSTRAINT diameter FOREIGN KEY(diameter_id) REFERENCES bolt_diameters(id)
);