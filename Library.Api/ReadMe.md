# Library Management System — .NET Minimal Web API

A backend API for a small library to manage books, members, and the borrowing/returning workflow. Built as a Minimal API on .NET 9, with EF Core + PostgreSQL, the repository pattern, and a clean separation between API contracts, domain logic, and infrastructure.

## Table of Contents

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [1. Run PostgreSQL with Docker](#1-run-postgresql-with-docker)
  - [2. Apply EF Core Migrations](#2-apply-ef-core-migrations)
  - [3. Run the API](#3-run-the-api)
  - [4. Access Swagger](#4-access-swagger)
- [Seed Data](#seed-data)
- [Example API Requests](#example-api-requests)
- [Business Rules Reference](#business-rules-reference)
- [Error Response Format](#error-response-format)
- [Running Tests](#running-tests)
- [Assumptions](#assumptions)
- [Bonus Items Implemented](#bonus-items-implemented)

## Overview

The API supports three core resources:

- **Books** — create, read, update, delete, with copy tracking (`TotalCopies` / `AvailableCopies`).
- **Members** — register and manage library members, with an active/inactive flag.
- **Borrowings** — borrow and return books, with a 3-active-borrow limit per member, a 14-day due date, and borrowing history.

Business logic lives in an application service layer and the domain entities themselves (e.g. `Book.BorrowCopy()`), not in the API endpoints. Endpoints stay thin: they parse the request, call a service, and shape the HTTP response.

## Technologies Used

| Concern | Choice |
|---|---|
| Runtime | .NET 9, ASP.NET Core Minimal APIs |
| ORM | Entity Framework Core |
| Database | PostgreSQL 16 (via Docker) |
| DB Driver | Npgsql.EntityFrameworkCore.PostgreSQL |
| Validation | FluentValidation |
| API Docs | Swashbuckle (Swagger / OpenAPI) |
| Testing | xUnit, Moq |
| Data access pattern | Repository + Unit of Work |

## Project Structure

```
Library.Api/
  Endpoints/          Minimal API endpoint definitions (thin HTTP layer)
  Contracts/           Request/response DTOs, grouped by resource
    Books/
    Members/
    Borrowings/
  Domain/
    Entities/           Book, Member, Borrowing — behavior-rich domain models
    Enums/              BorrowingStatus
  Application/
    Services/           Business logic (BookService, MemberService, BorrowingService)
    Interfaces/          Service and repository abstractions
  Infrastructure/
    Data/               DbContext, EF Core entity configurations, migrations
    Repositories/       Repository implementations
  Program.cs

Library.Api.Tests/     xUnit test project covering core business rules
docker-compose.yml       PostgreSQL container definition
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker Desktop
- (Optional) `dotnet-ef` CLI tool: `dotnet tool install --global dotnet-ef`

### 1. Run PostgreSQL with Docker

From the solution root:

```bash
docker compose up -d
```

This starts a PostgreSQL 16 container using the settings in `docker-compose.yml`:

| Setting | Value |
|---|---|
| Host | localhost |
| Port | 5432 |
| Database | librarydb |
| Username | libraryuser |
| Password | librarypassword |

Verify it's running:

```bash
docker ps
```

### 2. Apply EF Core Migrations

From the `Library.Api` project directory (or set it as the default project in the Visual Studio Package Manager Console):

```bash
dotnet ef database update
```

Or, in Visual Studio's **Package Manager Console**:

```powershell
Update-Database
```

This creates the `librarydb` schema (Books, Members, Borrowings tables) and applies the unique indexes on `Isbn` and `Email`.

### 3. Run the API

From Visual Studio, press **F5** (or **Ctrl+F5** to run without debugging), or from the CLI:

```bash
dotnet run --project Library.Api
```

By default the API listens on `https://localhost:{port}` — the exact port is printed in the console output and defined in `Library.Api/Properties/launchSettings.json`.

### 4. Access Swagger

Once running, open:

```
https://localhost:{port}/swagger
```

Swagger UI lists all endpoints grouped by tag (Books, Members, Borrowings), with request/response schemas and status codes.

## Seed Data

On startup in the Development environment, the API seeds a small sample dataset if the `Books` table is empty:

- 3 sample books (varying `TotalCopies`)
- 2 sample members (one active, one inactive)

This lets you exercise the borrowing endpoints immediately after the first run without manually creating records. See `Infrastructure/Data/LibraryDbContext` seeding logic (or the `HasData()` calls in the entity configurations, depending on which seeding approach was used) for the exact values.

To reset seed data, drop and recreate the database:

```bash
dotnet ef database drop
dotnet ef database update
```

## Example API Requests

### Create a book

```http
POST /api/books
Content-Type: application/json

{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "9780132350884",
  "publishedYear": 2008,
  "totalCopies": 3
}
```

### Register a member

```http
POST /api/members
Content-Type: application/json

{
  "fullName": "Jane Doe",
  "email": "jane.doe@example.com",
  "phoneNumber": "+1-555-0100"
}
```

### Borrow a book

```http
POST /api/borrowings
Content-Type: application/json

{
  "bookId": 1,
  "memberId": 1
}
```

### Return a book

```http
POST /api/borrowings/1/return
```

### Get a member's borrowing history

```http
GET /api/members/1/borrowings
```

A full set of runnable requests is available in `Library.Api.http` — open it in Visual Studio and use "Send Request" above each block.

## Business Rules Reference

| Rule | Enforced In |
|---|---|
| Title, Author, Isbn are required | `CreateBookRequestValidator` |
| Isbn is unique | `BookService.CreateAsync` (checked via `IBookRepository.GetByIsbnAsync`) → `409 Conflict` |
| PublishedYear cannot be in the future | `Book` constructor + validator |
| TotalCopies > 0 | `Book` constructor + validator |
| AvailableCopies cannot exceed TotalCopies | `Book.ReturnCopy()` |
| Email is required and valid | `CreateMemberRequestValidator` |
| Email is unique | `MemberService.CreateAsync` → `409 Conflict` |
| New members are active by default | `Member` constructor |
| Inactive member cannot borrow | `BorrowingService.BorrowBookAsync` |
| Book can be borrowed only if AvailableCopies > 0 | `Book.BorrowCopy()` |
| Member cannot exceed 3 active borrowings | `BorrowingService.BorrowBookAsync` |
| Borrowing reduces AvailableCopies by 1 | `Book.BorrowCopy()` |
| Returning increases AvailableCopies by 1 | `Book.ReturnCopy()` |
| A book cannot be returned twice | `Borrowing.MarkReturned()` |
| Due date is 14 days from borrow date | `Borrowing` constructor |

## Error Response Format

All errors follow a consistent shape, produced by the global exception-handling middleware:

```json
{
  "statusCode": 404,
  "message": "Book not found",
  "traceId": "00-abc123..."
}
```

Validation errors include a per-field breakdown:

```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": [
    { "field": "email", "message": "Email is required" }
  ]
}
```

| Scenario | Status Code |
|---|---|
| Created | 201 |
| Read / updated (with body) | 200 |
| Updated (no body) / Deleted | 204 |
| Validation failure | 400 |
| Not found | 404 |
| Duplicate ISBN or email | 409 |
| Business rule violation | 400 / 409 |


