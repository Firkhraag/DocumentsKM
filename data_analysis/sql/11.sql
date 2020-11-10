CREATE TABLE additional_mark_work (
	id serial PRIMARY KEY,
	mark_id int NOT NULL,
	employee_id int NOT NULL,
	order_num smallint NOT NULL,
	UNIQUE(mark_id, employee_id)
);