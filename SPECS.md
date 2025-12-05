# 📋 API Specifications

This document provides detailed specifications for all API endpoints, request/response formats, and business rules.

---

## Base URL

**Development:**
```
https://localhost:7286
http://localhost:5034
```

**Production:**
```
https://your-api-domain.com
```

---

## Authentication

Currently, this boilerplate does not implement authentication. All endpoints are publicly accessible.

**To add authentication:**
- Implement JWT Bearer tokens
- Add `[Authorize]` attributes to controllers
- Configure authentication in `Program.cs`

---

## Response Format

All endpoints return responses in JSON format following the `OperationResult` pattern:

### Success Response
```json
{
  "isSuccess": true,
  "data": { /* response data */ },
  "errorMessage": null,
  "errors": []
}
```

### Error Response
```json
{
  "isSuccess": false,
  "data": null,
  "errorMessage": "Error description",
  "errors": [
    "Detailed error 1",
    "Detailed error 2"
  ]
}
```

### Validation Error Response
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": [
    "Title is required",
    "Title cannot exceed 200 characters"
  ]
}
```

---

## Endpoints

### Health Check

#### Get API Health Status
```http
GET /health
```

**Description:** Check if the API is running and healthy.

**Response:** `200 OK`
```json
{
  "status": "healthy",
  "timestamp": "2024-11-24T15:30:00Z",
  "environment": "Development"
}
```

---

## TodoItems Resource

### Get All Todo Items
```http
GET /api/todo
```

**Description:** Retrieves all todo items.

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "title": "Buy groceries",
      "description": "Milk, bread, eggs",
      "isCompleted": false,
      "dueDate": "2024-11-30T00:00:00Z",
      "createdAt": "2024-11-24T10:00:00Z",
      "updatedAt": null
    },
    {
      "id": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
      "title": "Finish project",
      "description": "Complete Clean Architecture boilerplate",
      "isCompleted": true,
      "dueDate": null,
      "createdAt": "2024-11-20T14:30:00Z",
      "updatedAt": "2024-11-24T09:00:00Z"
    }
  ],
  "errorMessage": null,
  "errors": []
}
```

**Response (Empty):** `200 OK`
```json
{
  "isSuccess": true,
  "data": [],
  "errorMessage": null,
  "errors": []
}
```

---

### Get Todo Item by ID
```http
GET /api/todo/{id}
```

**Description:** Retrieves a specific todo item by its ID.

**Path Parameters:**
- `id` (Guid, required) - The unique identifier of the todo item

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Buy groceries",
    "description": "Milk, bread, eggs",
    "isCompleted": false,
    "dueDate": "2024-11-30T00:00:00Z",
    "createdAt": "2024-11-24T10:00:00Z",
    "updatedAt": null
  },
  "errorMessage": null,
  "errors": []
}
```

**Response (Not Found):** `404 Not Found`
```json
{
  "isSuccess": false,
  "data": null,
  "errorMessage": "TodoItem with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found",
  "errors": [
    "TodoItem with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found"
  ]
}
```

---

### Create Todo Item
```http
POST /api/todo
```

**Description:** Creates a new todo item.

**Request Body:**
```json
{
  "title": "Buy groceries",
  "description": "Milk, bread, eggs",
  "dueDate": "2024-11-30T00:00:00Z"
}
```

**Request Fields:**
| Field | Type | Required | Constraints | Description |
|-------|------|----------|-------------|-------------|
| `title` | string | Yes | Max 200 characters | The title of the todo item |
| `description` | string | No | Max 1000 characters | Additional details |
| `dueDate` | datetime | No | Must be in the future | When the todo is due |

**Response:** `201 Created`
```json
{
  "isSuccess": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Buy groceries",
    "description": "Milk, bread, eggs",
    "isCompleted": false,
    "dueDate": "2024-11-30T00:00:00Z",
    "createdAt": "2024-11-24T10:00:00Z",
    "updatedAt": null
  },
  "errorMessage": null,
  "errors": []
}
```

**Response (Validation Error):** `400 Bad Request`
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": [
    "Title is required",
    "Title cannot exceed 200 characters"
  ]
}
```

---

### Update Todo Item
```http
PUT /api/todo/{id}
```

**Description:** Updates an existing todo item.

**Path Parameters:**
- `id` (Guid, required) - The unique identifier of the todo item

**Request Body:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Buy groceries (updated)",
  "description": "Milk, bread, eggs, cheese",
  "isCompleted": true,
  "dueDate": "2024-11-30T00:00:00Z"
}
```

**Request Fields:**
| Field | Type | Required | Constraints | Description |
|-------|------|----------|-------------|-------------|
| `id` | Guid | Yes | Must match path parameter | The ID of the todo item |
| `title` | string | Yes | Max 200 characters | The title of the todo item |
| `description` | string | No | Max 1000 characters | Additional details |
| `isCompleted` | boolean | Yes | - | Completion status |
| `dueDate` | datetime | No | Must be in the future | When the todo is due |

**Response:** `204 No Content`

**Response (Not Found):** `404 Not Found`
```json
{
  "isSuccess": false,
  "data": null,
  "errorMessage": "TodoItem with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found",
  "errors": [
    "TodoItem with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found"
  ]
}
```

**Response (ID Mismatch):** `400 Bad Request`
```json
{
  "isSuccess": false,
  "data": null,
  "errorMessage": "ID mismatch",
  "errors": ["ID mismatch"]
}
```

**Response (Validation Error):** `400 Bad Request`
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": [
    "Title is required"
  ]
}
```

---

### Delete Todo Item
```http
DELETE /api/todo/{id}
```

**Description:** Deletes a todo item.

**Path Parameters:**
- `id` (Guid, required) - The unique identifier of the todo item

**Response:** `204 No Content`

**Response (Not Found):** `404 Not Found`
```json
{
  "isSuccess": false,
  "data": null,
  "errorMessage": "TodoItem with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found",
  "errors": [
    "TodoItem with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found"
  ]
}
```

---

## HTTP Status Codes

| Status Code | Meaning | Usage |
|-------------|---------|-------|
| `200 OK` | Success | Successful GET requests |
| `201 Created` | Resource created | Successful POST requests |
| `204 No Content` | Success with no body | Successful PUT/DELETE requests |
| `400 Bad Request` | Validation error | Invalid request data |
| `404 Not Found` | Resource not found | Requested resource doesn't exist |
| `500 Internal Server Error` | Server error | Unexpected server errors |

---

## Business Rules

### Todo Items

1. **Title is required** - Cannot be empty or null
2. **Title length** - Maximum 200 characters
3. **Description length** - Maximum 1000 characters (optional)
4. **Due date** - Must be in the future if provided
5. **IsCompleted** - Defaults to `false` on creation
6. **Timestamps** - `CreatedAt` is set automatically, `UpdatedAt` is set on updates

### Validation Rules

All validation is handled automatically via FluentValidation:

**CreateTodoItemCommand:**
```csharp
- Title: NotEmpty, MaxLength(200)
- Description: MaxLength(1000) when provided
- DueDate: GreaterThan(DateTime.UtcNow) when provided
```

**UpdateTodoItemCommand:**
```csharp
- Id: NotEmpty
- Title: NotEmpty, MaxLength(200)
- Description: MaxLength(1000) when provided
- DueDate: GreaterThan(DateTime.UtcNow) when provided
```

**DeleteTodoItemCommand:**
```csharp
- Id: NotEmpty
```

---

## Error Handling

### Global Exception Handler

All unhandled exceptions are caught by the `ExceptionHandlingMiddleware` and returned in a consistent format:

**Validation Exception:**
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": ["List of validation errors"]
}
```

**Not Found Exception:**
```json
{
  "statusCode": 404,
  "message": "Resource not found"
}
```

**Unauthorized Exception:**
```json
{
  "statusCode": 401,
  "message": "Unauthorized access"
}
```

**Internal Server Error:**
```json
{
  "statusCode": 500,
  "message": "An internal server error occurred",
  "detail": "Error details (development only)"
}
```

---

## Sample Requests

### cURL Examples

**Create a Todo:**
```bash
curl -X POST https://localhost:7286/api/todo \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Learn Clean Architecture",
    "description": "Study the boilerplate structure",
    "dueDate": "2024-12-01T00:00:00Z"
  }'
```

**Get All Todos:**
```bash
curl -X GET https://localhost:7286/api/todo
```

**Get Todo by ID:**
```bash
curl -X GET https://localhost:7286/api/todo/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Update a Todo:**
```bash
curl -X PUT https://localhost:7286/api/todo/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Content-Type: application/json" \
  -d '{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Learn Clean Architecture (updated)",
    "description": "Completed studying the structure",
    "isCompleted": true,
    "dueDate": "2024-12-01T00:00:00Z"
  }'
```

**Delete a Todo:**
```bash
curl -X DELETE https://localhost:7286/api/todo/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

---

## Future Enhancements

Potential additions to the API specifications:

- **Filtering** - Filter by completion status, date range
- **Sorting** - Sort by title, date, completion status
- **Pagination** - Limit results per page
- **Search** - Full-text search on title and description
- **Categories/Tags** - Group todos by category
- **Authentication** - JWT bearer tokens for user-specific todos
- **API Versioning** - Support multiple API versions

---

## Testing the API

**Swagger UI:**
Navigate to `https://localhost:7286/swagger` to interactively test all endpoints.

**Postman Collection:**
Import the API into Postman using the OpenAPI specification at:
```
https://localhost:7286/swagger/v1/swagger.json
```
