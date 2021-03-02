CREATE TABLE standard_constructions (
	id smallserial PRIMARY KEY,
	specification_id int NOT NULL,
	name varchar(255) NOT NULL,
	num smallint NOT NULL,
	sheet varchar(255),
	weight real NOT NULL,
	UNIQUE (specification_id, name),
	CONSTRAINT fk_specification FOREIGN KEY(specification_id) REFERENCES specifications(id)
);