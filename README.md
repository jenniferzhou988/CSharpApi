# ShoppingCart API

A **.NET 9** Web API powering a shopping cart application with JWT authentication, product catalog management, and Entity Framework Core backed by SQL Server.

**Repository:** [https://github.com/jenniferzhou988/CSharpApi](https://github.com/jenniferzhou988/CSharpApi) — branch: `development`

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Application Onboarding](#application-onboarding)
- [API Controllers](#api-controllers)
- [Data Model](#data-model)
- [Project Structure](#project-structure)

---

## Tech Stack

| Technology | Purpose |
|---|---|
| .NET 9 / ASP.NET Core | Web API framework |
| Entity Framework Core | ORM & database migrations (SQL Server) |
| JWT Bearer + Refresh Tokens | Authentication with secure token rotation |
| BCrypt | Password hashing (work factor 12) |
| Swagger / OpenAPI | Interactive API documentation |
| Rate Limiting | Fixed-window limits on auth endpoints |

---

## Application Onboarding

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) (local or remote)
- [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet)

Install the EF Core tools globally:

### 1. Clone the Repository

````````

### 2. Configure the Connection String

Edit `appsettings.json` and set your SQL Server connection string:

````````

### 3. Configure JWT Settings

Update the JWT section in `appsettings.json` with a strong signing key:

````````

### 4. Entity Framework Core — Database Migration

The project uses `GdctContext` as the EF Core `DbContext`, registered in `Program.cs`:

````````

Run the following commands to create and apply migrations:

````````


# Response

````````

- Swagger UI: `https://localhost:<port>/swagger`
- Health check: `GET /health` returns `{ "ok": true }`

---

## API Controllers

### AuthController — `api/Auth`

Handles user registration, login, JWT issuance, and refresh token rotation. All auth endpoints are rate-limited.

| Method | Route | Auth | Rate Limit | Description |
|---|---|---|---|---|
| POST | `api/Auth/register` | No | `auth` (100/10min) | Register a new user account |
| POST | `api/Auth/login` | No | `login` (10/10min) | Login with email and password |
| POST | `api/Auth/refresh` | No | `auth` | Rotate refresh token for a new token pair |
| POST | `api/Auth/logout` | No | `auth` | Revoke a single refresh token |
| POST | `api/Auth/logout-all` | Yes | `auth` | Revoke all refresh tokens for the current user |

**Register request:**

````````

**Login request:**

````````


# Response

````````

**Auth response (register and login):**

````````


# Response
````````

> When `UseCookiesForRefreshToken` is `true` in `appsettings.json`, the refresh token is sent and read via an HttpOnly cookie (`rt`) instead of the request body.

---

### ProfileController — `api/Profile`

Returns the authenticated user's profile. Requires a valid JWT Bearer token.

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `api/Profile/me` | Yes | Get the current user's profile |

**Response:**
````````


# Response
````````

---

### ProductController — `api/Product`

Full CRUD for products, plus sub-resource management for product images and category assignments.

#### Product CRUD

| Method | Route | Description |
|---|---|---|
| GET | `api/Product` | Get all products (includes images and categories) |
| GET | `api/Product/{id}` | Get a single product by ID |
| POST | `api/Product` | Create a new product |
| PUT | `api/Product/{id}` | Update an existing product |
| DELETE | `api/Product/{id}` | Delete a product |

**Product request:**

````````


# Response
````````

#### Product Images

| Method | Route | Description |
|---|---|---|
| POST | `api/Product/{productId}/images` | Add an image to a product |
| DELETE | `api/Product/{productId}/images/{imageId}` | Remove an image from a product |

**Image request:**

````````


# Response
````````

#### Product Categories

| Method | Route | Description |
|---|---|---|
| POST | `api/Product/{productId}/categories/{categoryId}` | Link a category to a product (idempotent) |
| DELETE | `api/Product/{productId}/categories/{categoryId}` | Remove a category from a product |

---

## Data Model

### Entity Relationship Diagrams (ERDs)

[Download the latest ERD diagram](https://danniseltodo.com/erd/schema_latest.pdf)

### Database tables created by EF Core:

| Table | Entity | Description |
|---|---|---|
| `UserInfo` | `User` | User accounts with BCrypt-hashed passwords |
| `UserRole` | `UserRole` | Role lookup (e.g. Admin, Client User) |
| `RefreshTokens` | `RefreshToken` | Hashed refresh tokens for JWT rotation |
| `Products` | `Product` | Product catalog |
| `ProductImages` | `ProductImage` | Images linked to a product (one-to-many) |
| `ProductCategory` | `ProductCategory` | Product categories |
| `ProductCategoryLink` | `ProductCategoryLink` | Many-to-many join between Products and Categories |
| `AppConfigs` | `AppConfig` | Application configuration entries |

All domain entities inherit from `GDCTEntityBase<int>` which provides: `Id` (auto-increment PK), `Status`, `CreatedBy`, `Created`, `ModifiedBy`, `Modified`.

**Key relationships:**
- **Product** to **ProductImage**: One-to-many (cascade delete)
- **Product** to **ProductCategory**: Many-to-many via `ProductCategoryLink` (unique composite index, cascade delete on both FKs)
- **User** to **RefreshToken**: One-to-many (unique index on `TokenHash`)
- **UserRole** to **User**: One-to-many (cascade delete)

---

## Project Structure

````````


# Response

````````

---

## License

[Specify your license here.]
