CREATE TABLE construction_types (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(255) UNIQUE NOT NULL
);