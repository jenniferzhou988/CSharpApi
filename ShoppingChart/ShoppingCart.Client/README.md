# ShoppingCart.Client

An Angular 17 single-page application for the ShoppingCart e-commerce platform. It connects to the **ShoppingCart.Server** (.NET 9) REST API and provides authentication, product management, and shopping cart functionality using Angular Material.

## Tech Stack

| Layer              | Technology                               |
| ------------------ | ---------------------------------------- |
| Framework          | Angular 17.3 (standalone components)     |
| UI Library         | Angular Material 17.3                    |
| Styling            | SCSS                                     |
| HTTP               | `@angular/common/http` with JWT interceptor |
| Routing            | `@angular/router` with lazy-loaded routes |
| State              | `BehaviorSubject` + `localStorage`       |
| Backend API        | .NET 9 — `https://localhost:7263/api`    |

## Prerequisites

- [Node.js](https://nodejs.org/) v18+
- [Angular CLI](https://angular.io/cli) v17.3+
- The **ShoppingCart.Server** API running at `https://localhost:7263`

## Getting Started

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 17.3.17.

### Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

### Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

### Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.
Build artifacts are output to `dist/shopping-cart.client/`.

### Running Tests

### Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

### Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

### Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.

## Environment Configuration

| File                              | Purpose                                          |
| --------------------------------- | ------------------------------------------------ |
| `src/environments/environment.ts` | Development — API at `https://localhost:7263/api` |
| `src/environments/environment.prod.ts` | Production — API at relative `/api`         |

The production build automatically swaps the environment file via `fileReplacements` in `angular.json`.

## Project Structure

## Key Features

| Feature                | Implementation                                                                 |
| ---------------------- | ------------------------------------------------------------------------------ |
| **Login**              | Email/password form → `POST /api/Auth/login` → JWT + roles stored in `localStorage` |
| **Auth State**         | `AuthService` with `BehaviorSubject`, JWT role parsing, persisted in `localStorage` |
| **Auth Guard**         | `authGuard` protects `/products/**` routes — redirects to `/login`             |
| **Product CRUD**       | List (Material table) + Form (add/edit) with all fields from `Product` model   |
| **Product Images**     | Material dialog to add/remove images per product                               |
| **Product Import**     | Material dialog with import history table + new import record form             |
| **Inventory Display**  | Total inventory quantity displayed in the product list table                   |
| **Lazy Loading**       | All feature components use `loadComponent` for code splitting                  |
| **Material UI**        | Toolbar, sidenav, table, dialogs, forms, snackbars, icons, chips               |
| **JWT Interceptor**    | Automatically attaches `Authorization: Bearer <token>` to all API requests     |

## API Endpoints Consumed

All endpoints are defined in `src/app/core/constants/api-endpoints.ts`:

| Domain          | Method   | Endpoint                                        |
| --------------- | -------- | ------------------------------------------------ |
| **Auth**        | `POST`   | `/api/Auth/login`                                |
|                 | `POST`   | `/api/Auth/register`                             |
|                 | `POST`   | `/api/Auth/refresh`                              |
|                 | `POST`   | `/api/Auth/logout`                               |
| **Product**     | `GET`    | `/api/Product`                                   |
|                 | `GET`    | `/api/Product/:id`                               |
|                 | `POST`   | `/api/Product`                                   |
|                 | `PUT`    | `/api/Product/:id`                               |
|                 | `DELETE` | `/api/Product/:id`                               |
|                 | `POST`   | `/api/Product/:id/images`                        |
|                 | `DELETE` | `/api/Product/:productId/images/:imageId`        |
|                 | `POST`   | `/api/Product/:productId/categories/:categoryId` |
|                 | `DELETE` | `/api/Product/:productId/categories/:categoryId` |
|                 | `POST`   | `/api/Product/:id/import`                        |
| **Cart**        | `GET`    | `/api/ShoppingCart`                              |
|                 | `POST`   | `/api/ShoppingCart`                              |
|                 | `DELETE` | `/api/ShoppingCart/:id`                          |
|                 | `POST`   | `/api/ShoppingCart/:cartId/items`                |
|                 | `PUT`    | `/api/ShoppingCart/:cartId/items/:detailId`      |
|                 | `DELETE` | `/api/ShoppingCart/:cartId/items/:detailId`      |
| **Order**       | `GET`    | `/api/Order`                                     |
|                 | `POST`   | `/api/Order`                                     |
| **Shipping**    | `GET`    | `/api/ShippingTracking`                          |
|                 | `POST`   | `/api/ShippingTracking`                          |

## Route Map

| Path               | Component              | Guard       | Description              |
| ------------------ | ---------------------- | ----------- | ------------------------ |
| `/login`           | `LoginComponent`       | —           | Sign-in page             |
| `/products`        | `ProductListComponent` | `authGuard` | Product table            |
| `/products/new`    | `ProductFormComponent` | `authGuard` | Create new product       |
| `/products/edit/:id` | `ProductFormComponent` | `authGuard` | Edit existing product  |
| `/`                | Redirect → `/products` | —           |                          |
| `**`               | Redirect → `/products` | —           | Wildcard catch-all       |

## Repository

- **GitHub:** [https://github.com/jenniferzhou988/CSharpApi](https://github.com/jenniferzhou988/CSharpApi)
- **Branch:** `development`
