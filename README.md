# Demo REST API Backend

This demo REST API backend is built using ASP.NET Core and demonstrates the use of Docker, Docker Compose, and PostgreSQL.

## Features
- **Password Hashing and Salting**
- **Configurable JWT Authentication with cookie support**
- **Configurable refresh token system with rotation and reuse detection**
- **Permission-role based authorisation**
- **Docker & Docker Compose support**
- **Database with PostgreSQL**
- **Object-relational mapping with Entity Framework Core**

## Getting Started

1. **Clone the repository**:
```bash
git clone https://github.com/Syntrice/demo-backend.git
```

2. **Build the Docker images**:
```bash
docker-compose build
```

3. **Run the application**:
```bash
docker-compose up
```

Your API server and database will start as defined in `compose.yaml`.

## License

Licensed under the MIT License.
