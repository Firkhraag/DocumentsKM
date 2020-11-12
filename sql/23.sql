CREATE TABLE profile_classes (
	id smallserial PRIMARY KEY,
	name varchar(255) UNIQUE NOT NULL,
	note varchar(255)
);