CREATE TABLE steel (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(255) NOT NULL UNIQUE,
	standard varchar(50) NOT NULL,
	strength smallint
);