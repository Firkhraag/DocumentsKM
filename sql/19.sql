CREATE TABLE corrosion_protection_methods (
	id smallserial PRIMARY KEY,
	env_aggressiveness smallint NOT NULL,
	material smallint NOT NULL,
	name varchar(255) NOT NULL,
	status smallint NOT NULL,
	UNIQUE(
		env_aggressiveness, material, name
	)
);