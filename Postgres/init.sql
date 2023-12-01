\c events;

CREATE TABLE scheduled_events (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100),
    description VARCHAR(250),
    location VARCHAR(100),
    date TIMESTAMP,
    creation_time TIMESTAMP,
    reminder_time TIMESTAMP
);

CREATE TABLE event_subscription (
    id SERIAL PRIMARY KEY,
    email VARCHAR(100),
    event_id INT NOT NULL,
    CONSTRAINT fk_event FOREIGN KEY(event_id) REFERENCES scheduled_events(id)
);