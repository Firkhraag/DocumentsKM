CREATE TABLE docs (
	id serial PRIMARY KEY,
	mark_id int NOT NULL,
	num smallint NOT NULL,
	name varchar(255) NOT NULL,
	form real NOT NULL DEFAULT 1.0,
	creator_id smallint NOT NULL,
	inspector_id smallint,
	norm_contr_id smallint,
	type_id smallint NOT NULL,
	release_num smallint NOT NULL DEFAULT 0,
	num_of_pages smallint NOT NULL DEFAULT 0,
	note varchar(255),
	UNIQUE (mark_id, num, type_id),
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_creator FOREIGN KEY(creator_id) REFERENCES employees(id),
	CONSTRAINT fk_inspector FOREIGN KEY(inspector_id) REFERENCES employees(id),
	CONSTRAINT fk_norm_contr FOREIGN KEY(norm_contr_id) REFERENCES employees(id),
	CONSTRAINT fk_doc_type FOREIGN KEY(type_id) REFERENCES doc_types(id)
);