CREATE TABLE nodes (
	id smallint identity(1, 1) PRIMARY KEY,
	code varchar(10) NOT NULL,
	name varchar(255) NOT NULL,
	project_id smallint NOT NULL,
	chief_engineer_id smallint NOT NULL,
	CONSTRAINT fk_project FOREIGN KEY(project_id) REFERENCES projects(id),
	CONSTRAINT fk_employee FOREIGN KEY(chief_engineer_id) REFERENCES employees(id)
);