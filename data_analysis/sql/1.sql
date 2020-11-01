CREATE TABLE corr_prot_variants (
	id smallserial PRIMARY KEY,
	operating_zone smallint NOT NULL,
	gas_group smallint NOT NULL,
	env_aggressiveness smallint NOT NULL,
	material smallint NOT NULL,
	paintwork_type varchar(2) NOT NULL,
	paintwork_group smallint NOT NULL,
	paintwork_durability varchar(2) NOT NULL,
	paintwork_num_of_layers smallint NOT NULL,
	paintwork_primer_thickness smallint NOT NULL,
	primer_num_of_layers smallint NOT NULL,
	cleaning_degree_id smallint NOT NULL,
	UNIQUE(
		operating_zone, gas_group, env_aggressiveness,
		material, paintwork_type, paintwork_group, paintwork_durability
	),
	CONSTRAINT fk_cleaning_degree FOREIGN KEY(cleaning_degree_id) REFERENCES corr_prot_cleaning_degrees(id)
);