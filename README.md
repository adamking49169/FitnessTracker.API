# FitnessTracker Solution

This repository contains a multi-project .NET solution for a simple fitness tracking API and related services. The code is split into several projects to keep domain models, data access and API concerns separated.

## Projects

| Project | Description |
|---------|-------------|
| **FitnessTracker.API** | ASP.NET Core Web API exposing endpoints for nutrition, meal plans, exercises and weight tracking. Uses EF Core with SQL Server, Azure Cosmos DB and OpenAI. |
| **FitnessTracker.Core** | Domain models shared across the solution. |
| **FitnessTracker.Infrastructure** | Data access layer and services such as `PlanService` and `NutritionService`. Contains the EF `FitnessTrackerDbContext`. |
| **FitnessTracker.Functions** | Placeholder for Azure Functions (e.g. background cache loaders). |
| **FitnessTracker.Tests** | xUnit tests for API services. |

## Building and Running

1. Ensure the required environment variables are set (see below).
2. Restore and build the solution:

```bash
 dotnet build FitnessTracker.sln
```

3. Run the API:

```bash
 dotnet run --project src/FitnessTracker.API
```

Swagger UI will be available by default at `https://localhost:5001` in development.

## Environment Variables

The application uses several environment variables that can also be specified in `appsettings.Development.json`.

| Variable | Purpose |
|----------|---------|
| `SQLSERVER_CONNECTION` | SQL Server connection string for the EF Core context. |
| `COSMOS_CONNECTION` | Cosmos DB connection string. |
| `OPENAI_API_KEY` | API key used by `OpenAiMealPlanService`. |
| `NUTRITIONIX_APP_ID` | Nutritionix API identifier. |
| `NUTRITIONIX_APP_KEY` | Nutritionix API key. |
| `EDAMAM_APP_ID` | Edamam API identifier. |
| `EDAMAM_APP_KEY` | Edamam API key. |
| `JWT_KEY` | JWT signing key (required in production). |
| `JWT_ISSUER` | JWT issuer used for authentication. |
| `JWT_AUDIENCE` | JWT audience used for authentication. |

## API Endpoints (summary)

- `GET /api/Exercise` – List all exercises from Cosmos DB.
- `GET /api/Nutrition/barcode/{upc}` – Look up nutrition info for a UPC.
- `POST /api/Nutrition/manual` – Create a manual food entry.
- `POST /api/MealPlan/generate/{calories}` – Generate a meal plan using OpenAI.
- `GET /api/MealPlan/plan` – Retrieve the saved plan for the current user.
- `POST /api/MealPlan/plan` – Save or update a user plan.
- `POST /api/Weight` – Record a weight entry.
- `GET /api/Weight` – List weight history.

All endpoints (except the sample `WeatherForecast` controller) require authentication. Development configuration uses permissive policies so JWT tokens are optional when running locally.

## Running Tests

Execute the tests with:

```bash
 dotnet test
```

The test project uses xUnit and can be run from the repository root or the `tests` directory.

## Creating Pull Requests via GitHub CLI

For repositories where the [GitHub CLI](https://cli.github.com/) is configured, you can automate opening a pull request using the provided `create-pr.sh` script.

Run the script from the repository root after staging your changes:

```bash
./create-pr.sh
```

The script checks out `main`, creates a branch, commits the current working tree and opens a PR that is ready to be merged once any checks pass.

## Running in Docker

Build and run the API in a container using the provided `Dockerfile`:

```bash
docker build -t fitnesstracker-api .
docker run -p 8080:80 fitnesstracker-api
```

Pass any required environment variables (e.g. `SQLSERVER_CONNECTION`) with `-e` flags when starting the container.
