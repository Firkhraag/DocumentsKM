CREATE TABLE profile_types (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(30) NOT NULL UNIQUE
);