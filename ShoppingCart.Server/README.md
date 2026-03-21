# ShoppingCart API (ASP.NET Core 9)

This project is a .NET 9 Web API for authentication, product catalog management, shopping carts, orders, shipping tracking, and user profile access.

## Tech Stack

- ASP.NET Core Web API (`net9.0`)
- Entity Framework Core + SQL Server
- JWT authentication + refresh token rotation
- Swagger/OpenAPI
- BCrypt password hashing

## Project Structure

```text
ShoppingCart.Server/
|-- Contracts/                 # API request/response DTOs
|-- Controllers/               # REST API controllers
|-- Data/                      # DbContext and data access infrastructure
|-- Extensions/                # Service registration extensions
|-- Models/                    # Domain entities and base model types
|-- Properties/                # launchSettings and runtime profiles
|-- Repository/
|   |-- Interface/             # Repository contracts
|   `-- Repositories/          # Repository implementations
|-- Services/                  # Business and security services (token, encryption)
|-- Utils/                     # Utility helpers
|-- Program.cs                 # Application entry point and middleware pipeline
|-- appsettings.json           # Shared configuration
|-- appsettings.Development.json
|-- ShoppingCartApi.csproj     # Project file
`-- ShoppingCartApi.sln        # Solution file
```

Folder responsibilities:

- `Controllers`: Defines API routes and HTTP behaviors.
- `Repository`: Encapsulates data access and aggregate loading patterns.
- `Models`: Represents database entities and relationships used by EF Core.
- `Data`: Central EF Core mapping in `GdctContext` and supporting converters.
- `Services`: Authentication/token and encryption services.
- `Contracts`: External API input/output contracts used by controllers.

## API Overview

Base URL in local development is typically:

- `https://localhost:<port>`

Swagger UI is enabled:

- `GET /swagger`

Health endpoint:

- `GET /health`

### API Controllers

| Controller | Base Route | Endpoints | Notes |
|---|---|---|---|
| AuthController | `/api/Auth` | `POST /register`<br>`POST /login`<br>`POST /refresh`<br>`POST /logout`<br>`POST /logout-all` | Registration, login, refresh-token rotation, logout actions |
| ProfileController | `/api/Profile` | `GET /me` | Authorized endpoint to read current user profile |
| ProductController | `/api/Product` | `GET /`<br>`GET /{id}`<br>`POST /`<br>`PUT /{id}`<br>`DELETE /{id}`<br>`POST /{productId}/images`<br>`DELETE /{productId}/images/{imageId}`<br>`POST /{productId}/categories/{categoryId}`<br>`DELETE /{productId}/categories/{categoryId}`<br>`POST /{productId}/import` | Product CRUD, image and category linking, inventory import |
| ShoppingCartController | `/api/ShoppingCart` | `GET /`<br>`GET /{id}`<br>`POST /`<br>`DELETE /{id}`<br>`POST /{shoppingCartId}/items`<br>`PUT /{shoppingCartId}/items/{detailId}`<br>`DELETE /{shoppingCartId}/items/{detailId}` | Cart CRUD and line-item management |
| OrderController | `/api/Order` | `GET /`<br>`GET /{id}`<br>`POST /` | Creates orders from customer, card, address, and item list |
| ShippingTrackingController | `/api/ShippingTracking` | `GET /`<br>`GET /{id}`<br>`POST /` | Creates and tracks shipping with provider + tracking number |
| WeatherForecastController | `/WeatherForecast` | `GET /` | Sample/demo endpoint |

## Database Entities

All entities are managed through `GdctContext` and most inherit common audit/status fields from `GDCTEntityBase<TId>`:

- `Id`, `Status`, `CreatedBy`, `Created`, `ModifiedBy`, `Modified`

Primary entities in this API:

| Domain | Entities |
|---|---|
| Identity and access | `User`, `Role`, `UserRole`, `RefreshToken`, `UserCustomerLink` |
| Customer profile | `Customer`, `Address`, `AddressType`, `CustomerAddressLink` |
| Payments | `BillingMethod`, `BankCardInfo`, `CustomerBillingCardLink` |
| Catalog | `Product`, `ProductImage`, `ProductCategory`, `ProductCategoryLink`, `ProductImportRecord`, `ProductInventory` |
| Cart and ordering | `ShoppingCart`, `ShoppingCartDetail`, `Order`, `OrderDetail`, `OrderStatus` |
| Shipping | `ShippingServiceProvider`, `ShippingTracking`, `ShippingItemDetail` |
| Configuration | `AppConfig` |

## Entity Relationships

| Domain | Source Entity | Cardinality | Target Entity | Notes |
|---|---|---|---|---|
| Identity | `User` | 1..* | `RefreshToken` | One user can own multiple refresh tokens |
| Identity | `User` | *..* | `Role` | Implemented through `UserRole` |
| Identity | `User` | 1..1 | `UserCustomerLink` | One-to-one link row |
| Identity | `Customer` | 1..1 | `UserCustomerLink` | One customer linked to one user |
| Customer and addresses | `Customer` | 1..* | `CustomerAddressLink` | Customer can store multiple addresses |
| Customer and addresses | `Address` | 1..* | `CustomerAddressLink` | Address can be referenced by link rows |
| Customer and addresses | `AddressType` | 1..* | `CustomerAddressLink` | Billing/Shipping role of address |
| Payments | `BillingMethod` | 1..* | `BankCardInfo` | Card belongs to one billing method |
| Payments | `Customer` | 1..* | `CustomerBillingCardLink` | Customer can own multiple saved cards |
| Payments | `BankCardInfo` | 1..* | `CustomerBillingCardLink` | Card can be referenced by link rows |
| Product catalog | `Product` | 1..* | `ProductImage` | Product media |
| Product catalog | `Product` | *..* | `ProductCategory` | Implemented through `ProductCategoryLink` |
| Product catalog | `Product` | 1..* | `ProductImportRecord` | Inventory import history |
| Product catalog | `Product` | 1..* | `ProductInventory` | Inventory snapshots/adjustments |
| Shopping and orders | `Customer` | 1..* | `ShoppingCart` | Customer carts |
| Shopping and orders | `ShoppingCart` | 1..* | `ShoppingCartDetail` | Cart line items |
| Shopping and orders | `Product` | 1..* | `ShoppingCartDetail` | Product appears in cart details |
| Shopping and orders | `Customer` | 1..* | `Order` | Customer orders |
| Shopping and orders | `OrderStatus` | 1..* | `Order` | Current order state |
| Shopping and orders | `Order` | 1..* | `OrderDetail` | Order line items |
| Shopping and orders | `Product` | 1..* | `OrderDetail` | Product appears in order details |
| Shipping | `ShippingServiceProvider` | 1..* | `ShippingTracking` | Provider tracking records |
| Shipping | `OrderDetail` | 1..1 | `ShippingItemDetail` | One shipping detail per order detail |

## Onboarding

### 1) Prerequisites

- .NET SDK 9.x
- SQL Server instance
- (Optional) Angular client project if using SPA proxy

### 2) Configure app settings

Update `appsettings.json` values before running:

- `ConnectionStrings:GDCTConnection`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SigningKey`
- `Jwt:AccessTokenMinutes`
- `Jwt:RefreshTokenDays`
- `Encryption:Key`
- `UseCookiesForRefreshToken`
- `Cors:AllowedOrigins`

Recommended:

- Move secrets (connection strings, JWT signing key, encryption key) to user secrets or environment variables for non-local use.

### 3) Restore and run

```bash
dotnet restore
dotnet build
dotnet run
```

### 4) Verify startup

- Open Swagger: `https://localhost:<port>/swagger`
- Health check: `https://localhost:<port>/health`

### 5) First-time API flow

1. Register user: `POST /api/Auth/register`
2. Login: `POST /api/Auth/login`
3. Copy `accessToken`
4. In Swagger, click **Authorize** and set `Bearer <accessToken>`
5. Call `GET /api/Profile/me`

### 6) Database setup notes

- The project uses EF Core model configuration in `Data/GdctContext.cs`.
- If migrations are introduced, apply them with:

```bash
dotnet ef database update
```

If you do not use migrations, ensure the target database schema is created to match the current model.

## Seed Data Notes

The model includes seeded records for some lookup/reference tables, including:

- `Role`
- `AddressType`
- `OrderStatus`
- `BillingMethod`
- `ProductCategory`

This helps bootstrap core workflow data for local testing.
