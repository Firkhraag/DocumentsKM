CREATE TABLE primer (
	id smallint identity(1, 1) PRIMARY KEY,
	group_num smallint NOT NULL,
	name varchar(255) NOT NULL,
	can_be_primed boolean NOT NULL,
	priority smallint NOT NULL DEFAULT 0,
	UNIQUE(group_num, name)
);