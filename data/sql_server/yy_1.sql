CREATE TABLE welding_control (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(50) NOT NULL UNIQUE
);