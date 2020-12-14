CREATE TABLE attached_docs (
	id serial PRIMARY KEY,
	mark_id int NOT NULL,
	designation varchar(100) NOT NULL,
	name varchar(200) NOT NULL,
	note varchar(50),
	UNIQUE(mark_id, designation)
);