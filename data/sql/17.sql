CREATE TABLE corrosion_protection_coating (
	id smallserial PRIMARY KEY,
	lp_type varchar(2) NOT NULL,
	lp_group smallint NOT NULL,
	lp_durability varchar(2) NOT NULL,
	lp_name varchar(255) NOT NULL,
	lp_num_of_layers smallint,
	gr_group smallint NOT NULL,
	lp_can  boolean NOT NULL,
	priority smallint NOT NULL,
	UNIQUE(
		lp_type, lp_group, lp_durability, lp_name
	)
);