CREATE DATABASE MessageDb;

\connect MessageDb;

CREATE TABLE messages (
    id SERIAL PRIMARY KEY,
    text VARCHAR(128) NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL,
    "order" INT NOT NULL
);
