CREATE TABLE dict_gen_dir_points (
	id smallserial PRIMARY KEY,
	section_id smallint NOT NULL,
	title varchar(80) NOT NULL,
	text varchar,
	do_not_cut boolean NOT NULL,
	fixed_point smallint NOT NULL,
	order_num smallint NOT NULL,
	UNIQUE(section_id, title, order_num)
);