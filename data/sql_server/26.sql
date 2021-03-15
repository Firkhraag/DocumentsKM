CREATE TABLE employees (
	id int identity(1, 1) PRIMARY KEY,
	position_id smallint NOT NULL,
	fullname varchar(255) NOT NULL,
	department_id smallint NOT NULL,
	CONSTRAINT fk_position FOREIGN KEY(position_id) REFERENCES positions(id),
	CONSTRAINT fk_department FOREIGN KEY(department_id) REFERENCES departments(id)
);