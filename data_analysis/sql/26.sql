CREATE TABLE employees (
	id smallserial PRIMARY KEY,
	position_id smallint NOT NULL,
	name varchar(50) NOT NULL,
	department_id smallint NOT NULL,
	CONSTRAINT fk_position FOREIGN KEY(position_id) REFERENCES positions(id),
	CONSTRAINT fk_department FOREIGN KEY(department_id) REFERENCES departments(id)
);