CREATE TABLE linked_docs (
	id smallserial PRIMARY KEY,
	code varchar(4) NOT NULL UNIQUE,
	type_id smallint NOT NULL,
	designation varchar(40) NOT NULL,
	name varchar(255) NOT NULL,
	CONSTRAINT fk_linked_doc_type FOREIGN KEY(type_id) REFERENCES linked_doc_types(id)
);