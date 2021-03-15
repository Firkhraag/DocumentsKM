CREATE TABLE corr_prot_variants (
	id smallint identity(1, 1) PRIMARY KEY,
	operating_area_id smallint NOT NULL,
	gas_group_id smallint NOT NULL,
	env_aggressiveness_id smallint NOT NULL,
	construction_material_id smallint NOT NULL,
	paintwork_type_id smallint,
	paintwork_group smallint,
	paintwork_fastness_id smallint,
	paintwork_num_of_layers smallint,
	paintwork_primer_thickness smallint,
	primer_num_of_layers smallint,
	cleaning_degree_id smallint,
	status smallint NOT NULL,
	UNIQUE(
		operating_area_id, gas_group_id, env_aggressiveness_id,
		construction_material_id, paintwork_type_id, paintwork_group
	),
	CONSTRAINT fk_operating_area FOREIGN KEY(operating_area_id) REFERENCES operating_areas(id),
	CONSTRAINT fk_gas_group FOREIGN KEY(gas_group_id) REFERENCES gas_groups(id),
	CONSTRAINT fk_env_aggressiveness FOREIGN KEY(env_aggressiveness_id) REFERENCES env_aggressiveness(id),
	CONSTRAINT fk_construction_material FOREIGN KEY(construction_material_id) REFERENCES construction_materials(id),
	CONSTRAINT fk_paintwork_type FOREIGN KEY(paintwork_type_id) REFERENCES paintwork_types(id),
	CONSTRAINT fk_paintwork_fastness FOREIGN KEY(paintwork_fastness_id) REFERENCES paintwork_fastness(id),
	CONSTRAINT fk_cleaning_degree FOREIGN KEY(cleaning_degree_id) REFERENCES corr_prot_cleaning_degrees(id)
);