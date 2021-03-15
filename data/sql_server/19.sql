CREATE TABLE corr_prot_methods (
	id smallint identity(1, 1) PRIMARY KEY,
	env_aggressiveness_id smallint NOT NULL,
	construction_material_id smallint NOT NULL,
	name varchar(255) NOT NULL,
	status smallint NOT NULL,
	UNIQUE(
		env_aggressiveness_id, construction_material_id, name
	),
	CONSTRAINT fk_env_aggressiveness FOREIGN KEY(env_aggressiveness_id) REFERENCES env_aggressiveness(id),
	CONSTRAINT fk_construction_material FOREIGN KEY(construction_material_id) REFERENCES construction_materials(id)
);