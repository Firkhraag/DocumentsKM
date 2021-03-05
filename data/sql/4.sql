CREATE TABLE primer (
	id smallserial PRIMARY KEY,
	group_num smallint NOT NULL,
	name varchar(255) NOT NULL,
	can_be_primed boolean NOT NULL,
	priority smallint NOT NULL DEFAULT 0,
	UNIQUE(group_num, name)
);