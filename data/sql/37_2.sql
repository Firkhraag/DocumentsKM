CREATE TABLE general_data_sections (
	id smallserial PRIMARY KEY,
	name varchar(255) NOT NULL UNIQUE,
	order_num smallint NOT NULL UNIQUE
);