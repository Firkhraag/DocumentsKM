CREATE TABLE default_values (
	user_id smallserial PRIMARY KEY,
	department_id smallint,
	creator_id smallint,
	inspector_id smallint,
	norm_contr_id smallint,
	CONSTRAINT fk_user FOREIGN KEY(user_id) REFERENCES users(id),
	CONSTRAINT fk_department FOREIGN KEY(department_id) REFERENCES departments(id),
	CONSTRAINT fk_creator FOREIGN KEY(creator_id) REFERENCES employees(id),
	CONSTRAINT fk_inspector FOREIGN KEY(inspector_id) REFERENCES employees(id),
	CONSTRAINT fk_norm_contr FOREIGN KEY(norm_contr_id) REFERENCES employees(id)
);