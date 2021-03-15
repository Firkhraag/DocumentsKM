CREATE TABLE general_data_sections (
	id smallint identity(1, 1) PRIMARY KEY,
	name varchar(255) NOT NULL UNIQUE,
	order_num smallint NOT NULL UNIQUE
);