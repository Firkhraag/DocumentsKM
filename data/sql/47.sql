CREATE TABLE standard_constructions (
	id smallserial PRIMARY KEY,
	specification_id int NOT NULL,
	name varchar(255) NOT NULL,
	num smallint,
	sheet varchar(255),
	weight real,
	UNIQUE (specification_id, name, num),
	CONSTRAINT fk_specification FOREIGN KEY(specification_id) REFERENCES specifications(id)
);