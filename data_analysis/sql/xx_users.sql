CREATE TABLE users (
	id smallserial PRIMARY KEY,
	login varchar(255) NOT NULL,
	password varchar(255) NOT NULL,
	employee_id smallint NOT NULL,
	CONSTRAINT fk_employee FOREIGN KEY(employee_id) REFERENCES employees(id)
);