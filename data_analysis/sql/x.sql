CREATE TABLE doc_types (
	id smallserial PRIMARY KEY,
	name varchar(100) NOT NULL UNIQUE
);