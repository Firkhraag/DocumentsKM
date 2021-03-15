CREATE TABLE estimate_task (
	mark_id int PRIMARY KEY,
	task_text varchar NOT NULL,
	additional_text varchar,
	approval_employee_id smallint,
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_employee FOREIGN KEY(approval_employee_id) REFERENCES employees(id)
);