CREATE TABLE mark_approvals (
	mark_id int,
	employee_id int,
	PRIMARY KEY (mark_id, employee_id),
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_employee FOREIGN KEY(employee_id) REFERENCES employees(id)
);