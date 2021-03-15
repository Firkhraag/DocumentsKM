CREATE TABLE sheet_names (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(255) NOT NULL UNIQUE
);