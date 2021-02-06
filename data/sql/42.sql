CREATE TABLE profile_types (
	id smallserial PRIMARY KEY,
	name varchar(30) NOT NULL UNIQUE
);