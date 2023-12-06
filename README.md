Event scheduler:

A backend for scheduling events and getting notifications before they start.

The project has an asp.net server that exposes an api to interact with the database.
The database is postgres running in a container.

There are some optimizations in this project to allow it to scale well.
The main processing is the notification sending and scheduling.
Instead of scheduling tasks to run from the creation of events, the server periodically gets upcoming events and schedules tasks only for those.
Also it listens for any updates on events to not miss anything.
The second optimization is the locking. An improvement from regular locking is locking by a single event. The dictionary of current events to notify is managed by only locking single events so other events can still be processed.


To run simply do docker compose up.
