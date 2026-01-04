# Freight Calculator

A robust Web API for freight calculation and order processing built with **C# 14** and **.NET 10**. This project demonstrates the application of **Clean Architecture**, **DDD Lite**, **Design Patterns** (Strategy & Factory), and rigorous automated testing practices.

## Table of Contents

- [Prerequisites](#prerequisites)
- [How to Run](#how-to-run)
- [Project Structure](#project-structure)
- [Architecture & Design Principles](#architecture--design-principles)

## Prerequisites

Ensure you have the following installed to run this project efficiently:

- **[.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)** (or later)
- **[Docker](https://www.docker.com/)** (Optional, for containerization)
- **IDE:** [Visual Studio](https://visualstudio.microsoft.com), [Visual Studio Code](https://code.visualstudio.com/), or [Rider](https://www.jetbrains.com/rider/).

## How to Run

### 1. Clone the Repository

```bash
git clone https://github.com/kauatwn/FreightCalculator.git
```

### 2. Enter the Directory

```bash
cd FreightCalculator
```

### 3. Choose Execution Method

You can run the application using **Docker** (recommended) or **Locally**.

#### Option A: Run with Docker

1. Build the image:

    ```bash
    docker build -t freight-calculator -f src/FreightCalculator.API/Dockerfile .
    ```

2. Run the container:

    ```bash
    docker run --rm -it -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development --name freight-calculator freight-calculator
    ```

*The API documentation will be accessible at `http://localhost:8080/swagger`.*

#### Option B: Run Locally

1. Restore dependencies:

    ```bash
    dotnet restore
    ```

2. Run the API:

    ```bash
    dotnet run --project src/FreightCalculator.API
    ```

*The API documentation will be accessible at `http://localhost:5276/swagger`.*

### 4. Execute Tests

To validate the domain logic and services:

```bash
dotnet test
```

## Project Structure

The solution follows the **Clean Architecture** principles to ensure separation of concerns and testability.

```plaintext
FreightCalculator/
├── src/
│   ├── FreightCalculator.API/              # Entry point, Controllers, Configuration
│   ├── FreightCalculator.Application/      # Use Cases (Vertical Slices), Orchestration, DTOs
│   ├── FreightCalculator.Domain/           # Entities, Interfaces, Enums (Pure Logic)
│   └── FreightCalculator.Infrastructure/   # Implementations (Services, Factory, Options)
└── tests/
    └── FreightCalculator.UnitTests/
```

## Architecture & Design Principles

This repository prioritizes **software engineering quality** over simple functionality, following strict development guidelines.

> [!NOTE]
> **Documentation Strategy:** This project deliberately omits extensive OpenAPI/Swagger documentation to maintain focus on **Domain Logic** and **Unit Testing purity**.
>
> For a production-ready API reference, see the [InventoryManager](https://github.com/kauatwn/InventoryManager) repository.

### 1. Domain-Driven Design (DDD Lite)

The core logic resides entirely within the `Domain` layer.

- **Rich Domain Models:** Entities like `Order` and `OrderItem` enforce invariants (e.g., price must be > 0) directly in their constructors and methods, preventing invalid states ("Anti-Anemic Model").
- **Encapsulation:** Properties use `private set` to ensure changes only happen through valid business methods like `AddItem()`.
- **Domain Exceptions:** Custom exceptions are used to handle business rule violations explicitly.

### 2. Design Patterns (SOLID)

To avoid complex `if/else` chains for shipping logic, the **Strategy Pattern** was implemented.

| Pattern                  | Usage Scenario                                         | Implementation                     |
|:------------------------:|:------------------------------------------------------:|:-----------------------------------|
| **Strategy**             | Calculating costs differently for Standard vs. Express | `IShippingService` implementations |
| **Factory**              | Selecting the correct strategy at runtime              | `ShippingServiceFactory`           |
| **Options**              | Strongly typed configuration management                | `IOptions<ShippingSettings>`       |
| **Dependency Injection** | Decoupling layers                                      | `IServiceCollection` extensions    |

### 3. Test-Driven Development (TDD) Approach

All business rules are verified through **xUnit** and **Moq**.

- **Domain Tests:** Verify that Entities behave correctly in isolation (e.g., Total calculation, Validations).
- **Application Tests:** Use Mocks to verify that the `CreateOrderUseCase` orchestrates the `ShippingFactory` and `ShippingService` correctly without relying on external infrastructure.

### 4. CI/CD & Quality

The project includes a **GitHub Actions** workflow that ensures quality on every push:

- **Automated Testing:** Runs all unit tests.
- **Static Analysis:** Integrates with **SonarCloud** for code quality gates.
- **Docker Build:** Verifies that the container image builds successfully.
