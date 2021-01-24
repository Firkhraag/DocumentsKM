CREATE TABLE element_profiles (
	id smallserial PRIMARY KEY,
	name varchar(255) UNIQUE NOT NULL,
	note varchar(255)
);