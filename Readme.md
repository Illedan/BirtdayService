
This code is made to test docker and is not techincally good at the moment.

## Todos
- Create database if not found
- Batch insert
	- API
	- View
- Edit/delete
	- API
	- View
- Automate building image
- Tests
	- unit
	- integration
	- UI?
- Clean up code
- https / http => support both by redirect


## Database
Is coded to work against a sqlite db found at C:\sqlite\birtday.db

CREATE TABLE birthday (id integer primary key, location VARCHAR(50), name VARCHAR(50), birthday int, birthmonth int, birthyear int, deleted boolean, deletedreason varchar(100));

## Run

- Create database and table

Either:
- dotnet run

or: 
- docker build -t birthdayservice .
- docker run birthdayservice
