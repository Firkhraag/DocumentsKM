CREATE TABLE profile_classes (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(255) UNIQUE NOT NULL,
	note varchar(255)
);