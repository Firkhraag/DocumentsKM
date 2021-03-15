CREATE TABLE departments (
	id smallserial PRIMARY KEY,
	code varchar(6) NOT NULL UNIQUE,
	name varchar(255) NOT NULL UNIQUE,
	short_name varchar(50) NOT NULL UNIQUE
);