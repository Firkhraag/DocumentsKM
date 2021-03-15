CREATE TABLE users (
	id smallint identity(1, 1) PRIMARY KEY,
	login varchar(255) NOT NULL,
	password varchar(255) NOT NULL,
	employee_id smallint NOT NULL,
	CONSTRAINT fk_employee FOREIGN KEY(employee_id) REFERENCES employees(id)
);