CREATE TABLE doc_types (
	id smallserial PRIMARY KEY,
	code varchar(4) NOT NULL UNIQUE,
	name varchar(100) NOT NULL UNIQUE
);