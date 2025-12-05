# 🏛️ Architecture Documentation

This document provides a detailed explanation of the architectural decisions and patterns used in this Clean Architecture boilerplate.

---

## Table of Contents

1. [Clean Architecture Overview](#clean-architecture-overview)
2. [Layer Responsibilities](#layer-responsibilities)
3. [Data Flow](#data-flow)
4. [Design Patterns](#design-patterns)
5. [Key Components](#key-components)
6. [Design Decisions](#design-decisions)

---

## Clean Architecture Overview

This boilerplate follows **Clean Architecture** (also known as Onion Architecture or Hexagonal Architecture) principles, which emphasize:

- **Independence of Frameworks** - Business logic doesn't depend on external libraries
- **Testability** - Business rules can be tested without UI, database, or external services
- **Independence of UI** - UI can change without affecting business rules
- **Independence of Database** - Can swap out databases without changing business logic
- **Independence of External Services** - Business rules don't know about external services

### The Dependency Rule

**Dependencies point inward.** Source code dependencies can only point inward toward higher-level policies.
```
┌─────────────────────────────────────────────┐
│                                             │
│  ┌───────────────────────────────────────┐  │
│  │                                       │  │
│  │  ┌─────────────────────────────────┐  │  │
│  │  │                                 │  │  │
│  │  │  ┌───────────────────────────┐  │  │  │
│  │  │  │                           │  │  │  │
│  │  │  │       Domain              │  │  │  │
│  │  │  │   (Entities, Rules)       │  │  │  │
│  │  │  │                           │  │  │  │
│  │  │  └───────────────────────────┘  │  │  │
│  │  │                                 │  │  │
│  │  │       Application               │  │  │
│  │  │   (Use Cases, Services)         │  │  │
│  │  │                                 │  │  │
│  │  └─────────────────────────────────┘  │  │
│  │                                       │  │
│  │         Infrastructure                │  │
│  │  (Repositories, External Services)    │  │
│  │                                       │  │
│  └───────────────────────────────────────┘  │
│                                             │
│              Presentation                   │
│         (Controllers, UI)                   │
│                                             │
└─────────────────────────────────────────────┘
```

---

## Layer Responsibilities

### 1. Domain Layer (Core)

**Location:** `src/Domain/`

**Purpose:** Contains enterprise business rules and entities.

**Responsibilities:**
- Define domain entities
- Define domain interfaces (if needed)
- Contain business rules and validations
- **No dependencies on other layers**

**Example:**
```csharp
// Domain/Entities/TodoItem.cs
public class TodoItem : BaseEntity
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    // Pure business logic, no framework dependencies
}
```

**Key Point:** The domain layer should be framework-agnostic. You should be able to use these entities in any type of application.

---

### 2. Application Layer (Use Cases)

**Location:** `src/Application/`

**Purpose:** Contains application business rules and orchestrates the flow of data.

**Responsibilities:**
- Define use cases (Commands and Queries via CQRS)
- Define application interfaces (e.g., `IGenericRepository`)
- Implement business logic that doesn't belong in domain
- Define DTOs for data transfer
- Coordinate domain objects

**Dependencies:**
- ✅ Domain layer (entities)
- ❌ Infrastructure layer
- ❌ Presentation layer

**Example:**
```csharp
// Application/TodoItems/Commands/CreateTodoItem/CreateTodoItemCommandHandler.cs
public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, OperationResult<TodoItemDto>>
{
    private readonly IGenericRepository<TodoItem> _repository;

    // Uses interface from Application layer
    // Implementation comes from Infrastructure layer
}
```

**Key Point:** Application layer defines contracts (interfaces) that Infrastructure must implement.

---

### 3. Infrastructure Layer (External Concerns)

**Location:** `src/Infrastructure/`

**Purpose:** Implements interfaces defined in Application layer and handles external concerns.

**Responsibilities:**
- Implement repositories
- Configure Entity Framework Core
- Implement external service integrations
- Handle database migrations
- Implement caching, email, file storage, etc.

**Dependencies:**
- ✅ Application layer (implements interfaces)
- ✅ Domain layer (uses entities)

**Example:**
```csharp
// Infrastructure/Repositories/GenericRepository.cs
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    
    // Implements interface from Application layer
    // Uses Entity Framework Core (framework-specific)
}
```

**Key Point:** This is where framework-specific code lives. Easy to swap out (e.g., switch from EF Core to Dapper).

---

### 4. Presentation Layer (API)

**Location:** `src/Api/`

**Purpose:** Handles HTTP requests and responses.

**Responsibilities:**
- Define API endpoints (controllers)
- Handle HTTP concerns (status codes, headers)
- Configure middleware
- Configure dependency injection
- API documentation (Swagger)

**Dependencies:**
- ✅ Application layer (sends commands/queries)
- ✅ Infrastructure layer (for DI registration only)
- ❌ Domain layer (should not directly use entities)

**Example:**
```csharp
// Api/Controllers/TodoController.cs
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpPost]
    public async Task<ActionResult> Create(CreateTodoItemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
```

**Key Point:** Controllers are thin. They just route requests to Application layer via MediatR.

---

## Data Flow

### Request Flow (Example: Create Todo)
```
1. HTTP POST Request
   ↓
2. Controller (Api Layer)
   └─ Receives CreateTodoItemCommand
   ↓
3. MediatR sends command to handler
   ↓
4. LoggingBehaviour (Pipeline)
   └─ Logs request
   ↓
5. ValidationBehaviour (Pipeline)
   └─ Validates command with FluentValidation
   ↓
6. CreateTodoItemCommandHandler (Application Layer)
   └─ Business logic
   └─ Calls IGenericRepository<TodoItem>
   ↓
7. GenericRepository (Infrastructure Layer)
   └─ EF Core adds entity to DbContext
   └─ SaveChanges persists to database
   ↓
8. Handler returns OperationResult<TodoItemDto>
   ↓
9. Controller returns HTTP 201 Created
```

### Dependency Flow
```
Api → Application → Domain
  ↓
Infrastructure → Application (implements interfaces)
               → Domain (uses entities)
```

**Note:** Infrastructure depends on Application, NOT the other way around. This is **Dependency Inversion**.

---

## Design Patterns

### 1. CQRS (Command Query Responsibility Segregation)

Separates read and write operations into Commands and Queries.

**Commands** (Write operations):
```
Application/TodoItems/Commands/
├── CreateTodoItem/
│   ├── CreateTodoItemCommand.cs          # The command
│   ├── CreateTodoItemCommandHandler.cs   # Handles the command
│   └── CreateTodoItemCommandValidator.cs # Validates the command
```

**Queries** (Read operations):
```
Application/TodoItems/Queries/
├── GetTodoItems/
│   ├── GetTodoItemsQuery.cs              # The query
│   └── GetTodoItemsQueryHandler.cs       # Handles the query
```

**Benefits:**
- Clear separation between reads and writes
- Different optimization strategies for each
- Easier to scale (can optimize queries separately)
- Better testability

---

### 2. Mediator Pattern (via MediatR)

Decouples request senders from request handlers.

**Without MediatR:**
```csharp
public class TodoController
{
    private readonly ITodoService _service;
    // Controller directly depends on service
}
```

**With MediatR:**
```csharp
public class TodoController
{
    private readonly IMediator _mediator;
    // Controller only depends on IMediator
    // Handler is resolved at runtime
}
```

**Benefits:**
- Loose coupling
- Easy to add pipeline behaviors (logging, validation)
- Single Responsibility Principle
- Open/Closed Principle

---

### 3. Repository Pattern

Abstracts data access logic.
```csharp
// Application Layer - Defines interface
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    // ... other methods
}

// Infrastructure Layer - Implements interface
public class GenericRepository<T> : IGenericRepository<T>
{
    private readonly ApplicationDbContext _context;
    // EF Core implementation
}
```

**Benefits:**
- Decouples business logic from data access
- Easy to mock for testing
- Can switch data access technology easily
- Consistent API across entities

---

### 4. Pipeline Behavior Pattern

Adds cross-cutting concerns via MediatR pipeline.

**Logging Behavior:**
```csharp
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    // Automatically logs every request
}
```

**Validation Behavior:**
```csharp
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    // Automatically validates every request
}
```

**Flow:**
```
Request → LoggingBehaviour → ValidationBehaviour → Handler → Response
```

**Benefits:**
- Cross-cutting concerns in one place
- Don't repeat validation/logging in every handler
- Easy to add new behaviors (caching, retry, etc.)

---

### 5. Operation Result Pattern

Consistent response handling instead of throwing exceptions for business rule violations.
```csharp
public class OperationResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; }
}
```

**Instead of:**
```csharp
if (todo == null)
    throw new NotFoundException("Todo not found"); // Exception for flow control
```

**Use:**
```csharp
if (todo == null)
    return OperationResult<TodoItemDto>.Failure("Todo not found"); // Explicit result
```

**Benefits:**
- Exceptions for exceptional cases only
- Explicit success/failure handling
- Better performance (no exception overhead)
- Easier to reason about code flow

---

## Key Components

### MediatR Pipeline
```
Request
  ↓
LoggingBehaviour
  ↓ (logs request name and timing)
ValidationBehaviour
  ↓ (runs FluentValidation)
Handler
  ↓ (executes business logic)
Response
```

**Configuration:**
```csharp
// Application/DependencyInjection.cs
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
```

---

### FluentValidation

Validation rules defined separately from commands.
```csharp
public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
```

**Automatic validation** via `ValidationBehaviour` in MediatR pipeline.

---

### Generic Repository

Single implementation for all entities:
```csharp
// Register once
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Use for any entity
IGenericRepository<TodoItem> todoRepo;
IGenericRepository<User> userRepo;
IGenericRepository<Product> productRepo;
```

---

## Design Decisions

### 1. Why Interfaces in Application Layer?

**Decision:** Repository interfaces (`IGenericRepository`) are in the Application layer, not Domain.

**Reasoning:**
- Domain layer stays pure (no infrastructure concerns)
- Application layer defines what it needs
- More pragmatic and common in real-world projects
- Easier to understand for most developers

**Alternative:** Some purists put interfaces in Domain. Both are valid.

---

### 2. Why CQRS Without Event Sourcing?

**Decision:** Use CQRS pattern but not full Event Sourcing.

**Reasoning:**
- Simpler to implement and understand
- Still get benefits of read/write separation
- Event Sourcing adds complexity not needed for most projects
- Can add Event Sourcing later if needed

---

### 3. Why In-Memory Database by Default?

**Decision:** Use in-memory database for development, easy to switch to SQL Server.

**Reasoning:**
- Zero configuration for new developers
- Fast iteration during development
- Easy to run tests
- Production configuration is well-documented

---

### 4. Why Single Test Project?

**Decision:** One test project with folders vs. separate test projects per layer.

**Reasoning:**
- Simpler structure
- Faster test runs (single compilation)
- Easier navigation
- Still maintains clear separation via folders

---

### 5. Why Per-Layer Dependency Injection?

**Decision:** Each layer has its own `DependencyInjection.cs` file.

**Reasoning:**
- Each layer owns its own configuration
- Follows Single Responsibility Principle
- Easy to find where services are registered
- Scales well as project grows
- Industry standard approach

---

## Further Reading

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR/wiki)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
