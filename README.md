
## Features

- Create a new repair plan
- Add procedures to the plan
- Assign users to each procedure
- Remove a single user or all users from a procedure
- Data persistence using SQLite
- Fully functional frontend in React.js
- Dockerized for easy setup

## Requirements

- Docker Desktop
- Node.js (for frontend development if running without Docker)
- .NET 6 SDK (if running backend without Docker)

## Run with Docker

```bash
# From repository root
docker-compose up -d --build

# Frontend: http://localhost:3001
# Backend: http://localhost:10010
