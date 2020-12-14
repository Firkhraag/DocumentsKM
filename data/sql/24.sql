CREATE TABLE construction_subtypes (
	id smallserial PRIMARY KEY,
	type_id smallint NOT NULL,
	name varchar(255) NOT NULL,
	valuation varchar(10) NOT NULL,
	UNIQUE(type_id, name, valuation),
	CONSTRAINT fk_type FOREIGN KEY(type_id) REFERENCES construction_types(id)
);