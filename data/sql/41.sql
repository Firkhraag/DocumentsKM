CREATE TABLE steel (
	id smallserial PRIMARY KEY,
	name varchar(255) NOT NULL UNIQUE,
	standard varchar(50) NOT NULL,
	strength smallint
);