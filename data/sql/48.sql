CREATE TABLE construction_elements (
	id serial PRIMARY KEY,
	construction_id int NOT NULL,
	profile_class_id smallint NOT NULL,
	profile_name varchar(30) NOT NULL,
	symbol varchar(2) NOT NULL,
	weight real NOT NULL,
	surface_area real NOT NULL,
	profile_type_id smallint NOT NULL,
	steel_id smallint NOT NULL,
	length real NOT NULL,
	status smallint NOT NULL,
	CONSTRAINT fk_construction FOREIGN KEY(construction_id) REFERENCES constructions(id),
	CONSTRAINT fk_profile_class FOREIGN KEY(profile_class_id) REFERENCES profile_classes(id),
	CONSTRAINT fk_profile_type FOREIGN KEY(profile_type_id) REFERENCES profile_types(id),
	CONSTRAINT fk_steel FOREIGN KEY(steel_id) REFERENCES steel(id)
);