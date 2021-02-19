CREATE TABLE profiles (
	id smallserial PRIMARY KEY,
	class_id smallint NOT NULL,
	name varchar(30) NOT NULL,
	symbol varchar(2),
	weight real NOT NULL,
	area real NOT NULL,
	type_id smallint NOT NULL,
	UNIQUE(class_id, name, symbol),
	CONSTRAINT fk_class FOREIGN KEY(class_id) REFERENCES profile_classes(id),
	CONSTRAINT fk_type FOREIGN KEY(type_id) REFERENCES profile_types(id)
);