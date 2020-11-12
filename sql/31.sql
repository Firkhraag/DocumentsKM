CREATE TABLE oz_paintings (
	id smallserial PRIMARY KEY,
	name varchar(100) UNIQUE NOT NULL,
	short_name varchar(30) UNIQUE NOT NULL,
	include boolean NOT NULL
);