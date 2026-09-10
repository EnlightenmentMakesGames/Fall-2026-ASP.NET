# Gradebook API
Basic ASP.NET API for managing a grades list

If running this from localhost, you can use the format `curl -X `[command type]` http://localhost:`[port] to access its commands.

Commands are run by appending their syntax to the end of the curl command, provided that the command type was input correctly.
## Command Endpoints:
- `GET`, `/grades`
	- Returns a copy of the grades list
- `POST`, `/grades -H Content-Type: application/json" -d "{\"student\"\"`(student name string)`\",\"score\":`(score, must be an int 0-100)`}`
	- Validates a grade and adds it to the list. Will return an error if validation is incorrect.
- `POST`, `/save`
	- Writes to data/grades.json
- `POST`, `/load`
	- Loads data/grades.json