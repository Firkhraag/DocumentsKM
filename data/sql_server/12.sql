CREATE TABLE attached_docs (
	id int identity(1, 1) PRIMARY KEY,
	mark_id int NOT NULL,
	designation varchar(100) NOT NULL,
	name varchar(200) NOT NULL,
	note varchar(50),
	UNIQUE(mark_id, designation)
);