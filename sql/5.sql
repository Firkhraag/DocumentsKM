CREATE TABLE fire_hazard_categories (
	id smallserial PRIMARY KEY,
	category varchar(1) NOT NULL UNIQUE,
	name varchar(30) NOT NULL,
	description varchar(255)
);