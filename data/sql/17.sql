CREATE TABLE corr_prot_coatings (
	id smallserial PRIMARY KEY,
	paintwork_type_id smallint NOT NULL,
	paintwork_group smallint NOT NULL,
	paintwork_fastness_id smallint NOT NULL,
	name varchar(255) NOT NULL,
	paintwork_num_of_layers smallint,
	primer_group smallint NOT NULL,
	can_be_painted boolean NOT NULL,
	priority smallint NOT NULL,
	UNIQUE(
		paintwork_type_id, paintwork_group, paintwork_fastness_id, name
	),
	CONSTRAINT fk_paintwork_type FOREIGN KEY(paintwork_type_id) REFERENCES paintwork_types(id),
	CONSTRAINT fk_paintwork_fastness FOREIGN KEY(paintwork_fastness_id) REFERENCES paintwork_fastness(id)
);