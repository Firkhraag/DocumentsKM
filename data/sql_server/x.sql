CREATE TABLE doc_types (
	id smallint identity(1, 1) PRIMARY KEY,
	code varchar(4) NOT NULL UNIQUE,
	name varchar(100) NOT NULL UNIQUE
);