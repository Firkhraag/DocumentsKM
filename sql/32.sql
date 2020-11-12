CREATE TABLE oz_painting_thickness (
	id smallserial PRIMARY KEY,
	lim smallint NOT NULL,
	metal_thickness real NOT NULL,
	oz_thickness real NOT NULL,
	oz_expenditure real NOT NULL,
	UNIQUE(lim, metal_thickness)
);