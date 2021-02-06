CREATE TABLE departments (
	id smallserial PRIMARY KEY,
	short_name varchar(30) NOT NULL,
	long_name varchar(255) NOT NULL
);

INSERT INTO departments (short_name, long_name)  VALUES ('TD1', 'TestDepartment1');
INSERT INTO departments (short_name, long_name)  VALUES ('TD2', 'TestDepartment2');