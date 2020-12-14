CREATE TABLE mark_general_data_points (
	id smallserial PRIMARY KEY,
	mark_id smallint NOT NULL,
	section_id smallint NOT NULL,
	text varchar NOT NULL,
	order_num smallint NOT NULL,
	UNIQUE(mark_id, section_id, order_num),
	UNIQUE(mark_id, section_id, text),
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_section FOREIGN KEY(section_id) REFERENCES general_data_sections(id)
);