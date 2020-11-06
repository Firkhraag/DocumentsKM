CREATE TABLE mark_linked_docs (
	id serial PRIMARY KEY,
	mark_id int,
	linked_doc_id smallint,
	UNIQUE(mark_id, linked_doc_id)
);