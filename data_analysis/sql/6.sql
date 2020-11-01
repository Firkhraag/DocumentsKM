CREATE TABLE sheets (
	id serial PRIMARY KEY,
	mark_id int NOT NULL,
	num smallint NOT NULL,
	name varchar(255),
	form real NOT NULL DEFAULT 1.0,
	creator_id int,
	inspector_id int,
	norm_contr_id int,
	doc_type_id smallint NOT NULL DEFAULT 0,
	release_num smallint,
	num_of_sheets smallint,
	note varchar(255),
	UNIQUE (mark_id, num, doc_type_id),
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_creator FOREIGN KEY(creator_id) REFERENCES employees(id),
	CONSTRAINT fk_inspector FOREIGN KEY(inspector_id) REFERENCES employees(id),
	CONSTRAINT fk_norm_contr FOREIGN KEY(norm_contr_id) REFERENCES employees(id),
	CONSTRAINT fk_doc_type FOREIGN KEY(doc_type_id) REFERENCES doc_types(id)
);