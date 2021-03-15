CREATE TABLE oz_paintings (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(100) UNIQUE NOT NULL,
	short_name varchar(30) UNIQUE NOT NULL,
	include boolean NOT NULL
);