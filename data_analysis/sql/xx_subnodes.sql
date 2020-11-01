CREATE TABLE subnodes (
	id smallserial PRIMARY KEY,
	node_id smallint NOT NULL,
	code varchar(10) NOT NULL,
	name varchar(255) NOT NULL,
	CONSTRAINT fk_node FOREIGN KEY(node_id) REFERENCES nodes(id)
);