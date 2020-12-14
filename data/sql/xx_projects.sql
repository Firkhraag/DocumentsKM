CREATE TABLE projects (
	id smallserial PRIMARY KEY,
	base_series varchar(20) NOT NULL UNIQUE,
	name varchar(255) NOT NULL
);