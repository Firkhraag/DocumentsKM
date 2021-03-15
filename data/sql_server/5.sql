CREATE TABLE fire_hazard_categories (
	id smallint identity(1, 1) PRIMARY KEY,
	category varchar(1) NOT NULL UNIQUE,
	name varchar(30) NOT NULL,
	description varchar(255)
);