CREATE TABLE additional_work (
	id int identity(1, 1) PRIMARY KEY,
	mark_id int NOT NULL,
	employee_id int NOT NULL,
	valuation smallint NOT NULL,
	metal_order smallint NOT NULL,
	UNIQUE(mark_id, employee_id)
);