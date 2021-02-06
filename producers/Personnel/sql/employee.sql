CREATE TABLE employees (
	id smallserial PRIMARY KEY,
	short_name varchar(20) NOT NULL,
	full_name varchar(255) NOT NULL,
    department_id smallint NOT NULL,
    position_id smallint NOT NULL,
    CONSTRAINT fk_department FOREIGN KEY(department_id) REFERENCES departments(id),
    CONSTRAINT fk_position FOREIGN KEY(position_id) REFERENCES positions(id)
);

INSERT INTO employees (short_name, full_name, department_id, position_id)  VALUES ('TE1', 'TestEmployee1', 1, 1);
INSERT INTO employees (short_name, full_name, department_id, position_id)  VALUES ('TE2', 'TestEmployee2', 2, 2);