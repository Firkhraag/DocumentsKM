CREATE TABLE profiles (
	id smallserial PRIMARY KEY,
	class_id smallint NOT NULL,
	name varchar(30) NOT NULL,
	symbol varchar(2),
	weight real NOT NULL,
	area real NOT NULL,
	type_id smallint NOT NULL,
	UNIQUE(class_id, type_id, name, symbol)
);