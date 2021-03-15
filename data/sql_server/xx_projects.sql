CREATE TABLE projects (
	id smallint identity(1, 1) PRIMARY KEY,
	base_series varchar(20) NOT NULL UNIQUE,
	name varchar(255) NOT NULL
);