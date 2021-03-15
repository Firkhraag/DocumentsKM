CREATE TABLE construction_elements (
	id int identity(1, 1) PRIMARY KEY,
	construction_id int NOT NULL,
	profile_id int NOT NULL,
	steel_id smallint NOT NULL,
	length real NOT NULL,
	CONSTRAINT fk_construction FOREIGN KEY(construction_id) REFERENCES constructions(id),
	CONSTRAINT fk_profile FOREIGN KEY(profile_id) REFERENCES profiles(id),
	CONSTRAINT fk_steel FOREIGN KEY(steel_id) REFERENCES steel(id)
);