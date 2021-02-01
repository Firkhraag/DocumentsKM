CREATE TABLE positions (
	id smallserial PRIMARY KEY,
	short_name varchar(20) NOT NULL,
	long_name varchar(255) NOT NULL
);

INSERT INTO positions (short_name, long_name)  VALUES ('TP1', 'TestPosition1');
INSERT INTO positions (short_name, long_name)  VALUES ('TP2', 'TestPosition2');