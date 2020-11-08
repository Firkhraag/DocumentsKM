CREATE TABLE mark_operating_conditions (
	mark_id int PRIMARY KEY,
	safety_coeff real,
	temperature smallint,
	operating_area_id smallint,
	gas_group_id smallint,
	env_aggressiveness_id smallint,
	construction_material_id smallint,
	paintwork_type_id smallint,
	high_tensile_bolts_type_id smallint,
	CONSTRAINT fk_mark FOREIGN KEY(mark_id) REFERENCES marks(id),
	CONSTRAINT fk_operating_area FOREIGN KEY(operating_area_id) REFERENCES operating_areas(id),
	CONSTRAINT fk_gas_group FOREIGN KEY(gas_group_id) REFERENCES gas_groups(id),
	CONSTRAINT fk_env_aggressiveness FOREIGN KEY(env_aggressiveness_id) REFERENCES env_aggressiveness(id),
	CONSTRAINT fk_construction_material FOREIGN KEY(construction_material_id) REFERENCES construction_materials(id),
	CONSTRAINT fk_paintwork_type FOREIGN KEY(paintwork_type_id) REFERENCES paintwork_types(id),
	CONSTRAINT fk_high_tensile_bolts_type FOREIGN KEY(high_tensile_bolts_type_id) REFERENCES high_tensile_bolts_types(id)
);