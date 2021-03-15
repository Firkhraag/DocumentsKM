CREATE TABLE mark_designations (
	id smallint identity(1, 1) PRIMARY KEY,
	designation varchar(4) NOT NULL UNIQUE,
	name varchar(50) NOT NULL UNIQUE
);