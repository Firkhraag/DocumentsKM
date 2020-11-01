CREATE TABLE mark_designations (
	id smallserial PRIMARY KEY,
	designation varchar(4) NOT NULL UNIQUE,
	name varchar(50) NOT NULL UNIQUE
);