CREATE TABLE general_data_points (
	id smallint identity(1, 1) PRIMARY KEY,
	section_id smallint NOT NULL,
	title varchar(80) NOT NULL,
	text varchar,
	do_not_cut boolean NOT NULL,
	fixed_point smallint NOT NULL,
	order_num smallint NOT NULL,
	table_name varchar(10),
	UNIQUE(section_id, order_num),
	CONSTRAINT fk_section FOREIGN KEY(section_id) REFERENCES general_data_sections(id)
);