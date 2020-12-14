CREATE TABLE general_data_points (
	id smallserial PRIMARY KEY,
	user_id smallint NOT NULL,
	section_id smallint NOT NULL,
	text varchar NOT NULL,
	order_num smallint NOT NULL,
	UNIQUE(user_id, section_id, text),
	CONSTRAINT fk_user FOREIGN KEY(user_id) REFERENCES users(id),
	CONSTRAINT fk_section FOREIGN KEY(section_id) REFERENCES general_data_sections(id)
);