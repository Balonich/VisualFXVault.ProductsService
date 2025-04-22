# VisualFXVault.ProductsService

Products Microservice for E-Commerce website related to VFX.

Built with Layered Architecture.

Used technologies:

- MS SQL
- .NET 9
- MinimalAPI
- EF Core
- FluentValidations

## TODO

### Initial setup

- [x] Add Data Access Layer project
- [x] Add Business Logic Layer project
- [x] Add API Layer project

### Data Access Layer

- [x] Add `Product` Entity with the following properties:
  - `ProductID` (GUID, primary key)
  - `ProductName` (string)
  - `Category` (string)
  - `UnitPrice` (double, nullable)
  - `QuantityInStock` (int, nullable)
- [x] Add `ApplicationDbContext`
- [x] Add `IProductsRepository` with the following methods:
  - `GetProducts()` - to retrieve all products.
  - `GetProductByCondition()` - to retrieve a product by a specific condition.
  - `AddProduct()` - to add a new product.
  - `UpdateProduct()` - to update an existing product.
  - `DeleteProduct()` - to delete a product by its ID.
- [x] Implement `IProductsRepository` in the `ProductsRepository` class
- [x] Add `DependencyInjection` class

### Business Logic Layer

- [x] Add DTOs
  - [x] `ProductResponse`: Contains data to return in API responses.
  - [x] `ProductAddRequest`: For adding a new product.
  - [x] `ProductUpdateRequest`: For updating an existing product.
- [x] Add `ProductService`. This service will:
  - Retrieve all products or products by specific conditions.
  - Add, update, and delete products by interacting with the repository.
- [x] Create `ProductAddRequestValidator` and `ProductUpdateRequestValidator` with FluentValidation

### API Layer

- [x] Implement Minimal API endpoints:
  - [x] `GET /api/products`: Retrieve all products.
  - [x] `GET /api/products/search/product-id/{productId}`: Retrieve a product by ID.
  - [x] `GET /api/products/search/{searchString}`: Performs a "contains" search on product name or category name.
  - [x] `POST /api/products`: Add a new product.
  - [x] `PUT /api/products`: Update an existing product.
  - [x] `DELETE /api/products/{productId}`: Delete a product by ID.
- [x] Add exception handling middleware. This middleware should:
  - Log exceptions.
  - Return a generic error response with a 500 status code if any unhandled exceptions occur.
- [x] Add Scalar UI for OpenAPI documentation
