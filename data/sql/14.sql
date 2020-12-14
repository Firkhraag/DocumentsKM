CREATE TABLE mark_linked_docs (
	id smallserial PRIMARY KEY,
	mark_id int,
	linked_doc_id smallint,
	UNIQUE(mark_id, linked_doc_id),
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_linked_doc FOREIGN KEY(linked_doc_id) REFERENCES linked_docs(id)
);