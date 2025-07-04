# Demo REST API Backend

This demo REST API backend is built using ASP.NET Core and demonstrates the use of Docker, Docker Compose, and Microsoft SQL Server.

## Features
- **ASP.NET Core**: Robust framework for building web APIs.
- **Docker & Docker Compose**: Containerize and streamline your development and deployment.
- **MSSQL Server**: Integrated SQL database for backend operations.

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

## Testing

To run the test suite, execute:
```bash
dotnet test DemoBackend.Tests/DemoBackend.Tests.csproj
```

## Project Structure

- **DemoBackend/**: Main API project.
- **DemoBackend.Tests/**: Test project.
- **compose.yaml**: Docker Compose configuration.
- **Dockerfile**: Docker configuration for building the API image.

## License

Licensed under the MIT License.
