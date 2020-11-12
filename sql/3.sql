CREATE TABLE specifications (
	id serial PRIMARY KEY,
	mark_id int NOT NULL,
	num smallint NOT NULL,
	created_date timestamp DEFAULT NOW(),
	is_current boolean NOT NULL DEFAULT true,
	note varchar(255),
	UNIQUE (mark_id, num),
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id)
);