CREATE TABLE construction_subtypes (
	id smallserial PRIMARY KEY,
	construction_type_id smallint NOT NULL,
	code smallint NOT NULL,
	name varchar(255) NOT NULL,
	valuation varchar(10) NOT NULL,
	UNIQUE(construction_type_id, code, valuation)
);