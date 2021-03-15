CREATE TABLE general_guidance_points (
	id int identity(1, 1) PRIMARY KEY,
	mark_id int NOT NULL,
	point smallint NOT NULL,
	text varchar,
	include boolean NOT NULL,
	original boolean NOT NULL,
	do_not_cut boolean NOT NULL,
	point_type smallint NOT NULL DEFAULT 0,
	UNIQUE(mark_id, point)
);