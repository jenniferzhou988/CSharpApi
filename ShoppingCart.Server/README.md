Here's the improved `README.md` file that incorporates the new content regarding the `ProductCategory` entity and the `ProductCategoryLink` join entity while maintaining the existing structure and coherence:

# Project Title

## Description

[Provide a brief description of the project, its purpose, and any relevant information.]

## Getting Started

[Instructions on how to get the project up and running locally.]

## Added: Product model, repository and API

This section documents the recent additions: a `Product` entity, an EF Core integration, a repository, and an API controller.

### Files added / updated

- `Models/Product.cs` — `Product` entity with fields: `Id` (auto-increment), `ProductName`, `Price`, `Description`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy`.
- `Data/ApplicationDbContext.cs` — exposes `DbSet<Product> Products`, configures the entity and applies automatic `Created`/`Modified` timestamps in `SaveChanges`.
- `Repositories/IProductRepository.cs` — repository interface (async CRUD).
- `Repositories/ProductRepository.cs` — EF Core repository implementation.
- `Controllers/ProductController.cs` — Web API controller exposing CRUD endpoints.

### Dependency injection

Register the repository in `Program.cs` (or `Startup.cs`) so it is available via Dependency Injection (DI):

// Program.cs
builder.Services.AddScoped<AngularApplication.Repositories.IProductRepository, AngularApplication.Repositories.ProductRepository>();

Ensure your `ApplicationDbContext` is registered (example using SQL Server):

// Program.cs
builder.Services.AddDbContext<AngularApplication.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

### Database migration

To apply the changes to your database, add a migration and update the database:

dotnet ef migrations add AddProduct
dotnet ef database update

### API endpoints

Base route: `api/Product`

- **GET** `api/Product` — Retrieve all products.
- **GET** `api/Product/{id}` — Retrieve a product by its ID.
- **POST** `api/Product` — Create a new product (returns `201 Created`).
- **PUT** `api/Product/{id}` — Update an existing product (returns `200 OK`).
- **DELETE** `api/Product/{id}` — Delete a product (returns `204 No Content`).

### Sample request payload

Here is an example of a request payload for creating a new product:

{
  "productName": "Sample Product",
  "price": 19.99,
  "description": "Short description"
}

**Note:** `Id`, `Created`, and `Modified` are managed by the server. `CreatedBy` and `ModifiedBy` are present but not yet wired to an identity provider.

### Notes & next steps

- If you already have an existing `ApplicationDbContext`, merge the `DbSet<Product>` and entity configuration rather than replacing the file.
- To wire `CreatedBy` and `ModifiedBy`, consider adding a current-user provider and injecting it into `ApplicationDbContext`.
- Register the repository in DI and add any required authorization to the controller endpoints if needed.

## Added: ProductImage model, repository methods and API endpoints

This section documents the addition of a `ProductImage` entity linked to `Product` via a one-to-many relationship, along with supporting repository methods and API endpoints.

### Summary of changes

| File | Change |
|---|---|
| `Models/ProductImage.cs` | **New** — `ProductImage` entity inheriting `GDCTEntityBase<int>` with `ImageUrl`, `AltText`, and `ProductId` (FK). Table name: `ProductImages`. |
| `Models/Product.cs` | Added `Images` navigation property (`ICollection<ProductImage>`). |
| `Data/GdctContext.cs` | Added `DbSet<ProductImage> ProductImages`, Fluent API configuration for the `ProductImages` table, and one-to-many relationship with cascade delete. |
| `Repository/Interface/IProductRepository.cs` | Added `AddImageAsync` and `RemoveImageAsync` method signatures. |
| `Repository/Repositories/ProductRepository.cs` | Implemented `AddImageAsync` / `RemoveImageAsync`; added `.Include(p => p.Images)` to `GetByIdAsync` and `GetAllAsync`. |
| `Controllers/ProductController.cs` | Added `POST api/Product/{productId}/images` and `DELETE api/Product/{productId}/images/{imageId}` endpoints. |

### ProductImage entity

- **Table:** `ProductImages`
- **Inherits:** `GDCTEntityBase<int>` (provides `Id`, `Status`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy`)
- **Properties:**
  - `ImageUrl` (`string`, required, max 500) — URL of the image.
  - `AltText` (`string?`, max 200) — Optional alt text for accessibility.
  - `ProductId` (`int`, required) — Foreign key referencing `Product.Id`.

### New API endpoints

| Method | Route | Description |
|---|---|---|
| **POST** | `api/Product/{productId}/images` | Add an image to a product. |
| **DELETE** | `api/Product/{productId}/images/{imageId}` | Remove a specific image from a product. |

### Sample request — Add image

Here is an example of a request payload for adding an image to a product:

{
  "imageUrl": "https://example.com/images/product1.jpg",
  "altText": "Product front view"
}

### Database migration

To apply the `ProductImages` table to your database:

dotnet ef migrations add AddProductImages
dotnet ef database update

### Notes

- `GetByIdAsync` and `GetAllAsync` now eager-load `Images` via `.Include(p => p.Images)`.
- Deleting a `Product` will cascade-delete all associated `ProductImage` records.

## Added: ProductCategory entity and EF Core configuration

This section documents the addition of a `ProductCategory` entity linked to `Product` via a one-to-many relationship, along with the corresponding EF Core Fluent API configuration.

### Summary of changes

| File | Change |
|---|---|
| `Models/ProductCategory.cs` | **New** — `ProductCategory` entity inheriting `GDCTEntityBase<int>` with `CategoryName`, `Description`, and `Products` navigation collection. Table: `ProductCategory`. |
| `Data/GdctContext.cs` | Added `DbSet<ProductCategory> ProductCategories`, Fluent API configuration for the `ProductCategory` table, and one-to-many relationship (`ProductCategory` → `Product`) with `DeleteBehavior.SetNull`. |

### ProductCategory entity

- **Table:** `ProductCategory`
- **Inherits:** `GDCTEntityBase<int>` (provides `Id`, `Status`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy`)
- **Properties:**
  - `CategoryName` (`string`, required, max 200) — Name of the category.
  - `Description` (`string?`, max 1000) — Optional category description.
  - `Products` (`ICollection<Product>`) — Navigation collection of products in this category.

### Relationship details

- **Type:** One-to-many (`ProductCategory` → `Product`)
- **Foreign key:** `Product.ProductCategoryId` (nullable `int?`)
- **Delete behavior:** `SetNull` — deleting a category sets `ProductCategoryId` to `null` on related products rather than cascade-deleting them.
- `ProductCategoryId` is nullable so existing products are not required to have a category assigned.

### Database migration

To apply the `ProductCategory` table to your database:

dotnet ef migrations add AddProductCategory
dotnet ef database update

## Added: ProductCategoryLink join entity and category management endpoints

This section documents the addition of a `ProductCategoryLink` join entity that establishes a many-to-many relationship between `Product` and `ProductCategory`, along with repository methods and API endpoints to add or remove categories from a product.

### Summary of changes

| File | Change |
|---|---|
| `Models/ProductCategoryLink.cs` | **New** — Join entity inheriting `GDCTEntityBase<int>` with `ProductId` (FK) and `ProductCategoryId` (FK). Table: `ProductCategoryLink`. |
| `Models/Product.cs` | Removed direct `ProductCategoryId` / `ProductCategory` FK. Added `ProductCategoryLinks` navigation collection. |
| `Models/ProductCategory.cs` | Replaced `Products` navigation with `ProductCategoryLinks` navigation collection. |
| `Data/GdctContext.cs` | Added `DbSet<ProductCategoryLink> ProductCategoryLinks`. Added Fluent API config with unique composite index on `(ProductId, ProductCategoryId)` and two one-to-many relationships with cascade delete. Removed old direct `ProductCategory` → `Product` relationship. |
| `Repository/Interface/IProductRepository.cs` | Added `AddCategoryAsync(int productId, int categoryId)` and `RemoveCategoryAsync(int productId, int categoryId)` method signatures. |
| `Repository/Repositories/ProductRepository.cs` | Implemented `AddCategoryAsync` (validates product & category, prevents duplicates) and `RemoveCategoryAsync`. Added `.Include(p => p.ProductCategoryLinks).ThenInclude(l => l.ProductCategory)` to `GetByIdAsync` and `GetAllAsync`. |
| `Controllers/ProductController.cs` | Added `POST api/Product/{productId}/categories/{categoryId}` and `DELETE api/Product/{productId}/categories/{categoryId}` endpoints. |

### ProductCategoryLink entity

- **Table:** `ProductCategoryLink`
- **Inherits:** `GDCTEntityBase<int>` (provides `Id`, `Status`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy`)
- **Properties:**
  - `ProductId` (`int`, required) — Foreign key referencing `Product.Id`.
  - `ProductCategoryId` (`int`, required) — Foreign key referencing `ProductCategory.Id`.
  - `Product` — Navigation property to `Product`.
  - `ProductCategory` — Navigation property to `ProductCategory`.

### Relationship details

- **Type:** Many-to-many via explicit join entity (`Product` ↔ `ProductCategoryLink` ↔ `ProductCategory`)
- **Unique composite index** on `(ProductId, ProductCategoryId)` prevents duplicate links.
- **Delete behavior:** Cascade on both FKs — deleting a `Product` or `ProductCategory` removes the associated link rows.
- A product can belong to **multiple categories**, and a category can contain **multiple products**.

### New API endpoints

| Method | Route | Description |
|---|---|---|
| **POST** | `api/Product/{productId}/categories/{categoryId}` | Add a category to a product (idempotent — returns existing link if already assigned). |
| **DELETE** | `api/Product/{productId}/categories/{categoryId}` | Remove a category from a product. |

### Key behaviors

- **`AddCategoryAsync`** — Validates both product and category exist; returns the existing link if already present (idempotent); returns `null` if either entity is not found.
- **`RemoveCategoryAsync`** — Finds the link by `ProductId` + `ProductCategoryId` and removes it; returns `false` if link doesn't exist.
- **`GetByIdAsync` / `GetAllAsync`** — Eager-load `ProductCategoryLinks` with `.ThenInclude(l => l.ProductCategory)` so category details are included in API responses.

### Database migration

To apply the `ProductCategoryLink` table to your database:

dotnet ef migrations add AddProductCategoryLink
dotnet ef database update

## Contributing

[Instructions for contributing to the project.]

## License

[Information about the project's license.]

## Acknowledgments

[Credits and acknowledgments for resources, libraries, or individuals that contributed to the project.]