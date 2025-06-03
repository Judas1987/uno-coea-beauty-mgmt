# BeautyManagement

## Overview of Application

BeautyManagement is a simple admin portal for managing customer appointments, customer services, and service catalog. 

The application will allow an administrator to sign in, manage bookings via a calendar view, manage customers, services, and have access to basic reporting.

## Problem Definition

## Requirements Prioritised

### Must Have
CRUD operations for appointments
- Handle scheduling conflicts
- Integrate with notification service
- Include unit tests (xUnit/NUnit)

Manage customer profiles
- Track appointment history
- Implement loyalty points system
- Add search functionality

Maintain service inventory
- Manage pricing and packages
- Associate services with categories
- Handle seasonal promotions

Responsive dashboard
  - Calendar view (FullCalendar integration)
  - Customer management CRUD
  - Service configuration
  - Basic reporting

### Should Have

Unit Tests: 70%+ coverage per service
Integration Tests: API endpoint validation
E2E Tests: Cypress for Angular components
Load Testing: Locust for performance validation

### Could Have

Real-time updates with SignalR
Customer self-booking portal
Payment integration
CI/CD pipeline example
Performance monitoring

## Domain Model Diagram

- **Customers**: 

- **Appointments**: 

- **Packages**: 

### Glossary

## API Structure

### Services

#### `GET /api/Services`
**Description:** Get all services.

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Haircut",
    "description": "Basic haircut service",
    "price": 50.00,
    "durationMinutes": 30,
    "categoryId": 1,
    "categoryTitle": "Hair Care",
    "isActive": true,
    "isPromotional": false,
    "promotionalPrice": null
  }
]
```

#### `GET /api/Services/{id}`
**Description:** Get a service by ID.

**Parameters:**
- `id`: Service ID (path)

**Responses:**
- `200 OK`
- `404 Not Found`

#### `POST /api/Services`
**Description:** Create a new service.

**Request Body:**
```json
{
  "title": "Haircut",
  "description": "Basic haircut service",
  "price": 50.00,
  "durationMinutes": 30,
  "categoryId": 1
}
```

**Responses:**
- `201 Created`
- `400 Bad Request`

#### `PUT /api/Services/{id}`
**Description:** Update an existing service.

**Parameters:**
- `id`: Service ID (path)

**Request Body:**
```json
{
  "title": "Haircut",
  "description": "Updated description",
  "price": 55.00,
  "durationMinutes": 30,
  "categoryId": 1
}
```

**Responses:**
- `204 No Content`
- `400 Bad Request`
- `404 Not Found`

#### `DELETE /api/Services/{id}`
**Description:** Delete a service.

**Parameters:**
- `id`: Service ID (path)

**Responses:**
- `204 No Content`
- `404 Not Found`

#### `GET /api/Services/category/{categoryId}`
**Description:** Get services by category.

**Parameters:**
- `categoryId`: Category ID (path)

**Response:** `200 OK`

#### `GET /api/Services/promotions`
**Description:** Get all active promotional services.

**Response:** `200 OK`

#### `PATCH /api/Services/{id}/promotional-price`
**Description:** Set a promotional price for a service.

**Parameters:**
- `id`: Service ID (path)
- Body: Decimal value for promotional price

**Responses:**
- `204 No Content`
- `400 Bad Request`
- `404 Not Found`

#### `PATCH /api/Services/{id}/activate`
**Description:** Activate a service.

**Parameters:**
- `id`: Service ID (path)

**Responses:**
- `204 No Content`
- `404 Not Found`

#### `PATCH /api/Services/{id}/deactivate`
**Description:** Deactivate a service.

**Parameters:**
- `id`: Service ID (path)

**Responses:**
- `204 No Content`
- `404 Not Found`

#### `GET /api/Services/search`
**Description:** Search services by term.

**Parameters:**
- `searchTerm`: Search query (query)

**Response:** `200 OK`

#### `GET /api/Services/price-range`
**Description:** Get services within a price range.

**Parameters:**
- `minPrice`: Minimum price (query)
- `maxPrice`: Maximum price (query)

**Responses:**
- `200 OK`
- `400 Bad Request`

### Service Categories

#### `GET /api/ServiceCategories`
**Description:** Get all service categories.

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Hair Care",
    "description": "All hair-related services"
  }
]
```

### Appointments

#### `GET /api/Appointments`
**Description:** Get all appointments.

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "customerId": 1,
    "serviceId": 1,
    "startTime": "2024-03-20T10:00:00",
    "endTime": "2024-03-20T11:00:00",
    "status": "Scheduled",
    "notes": "First time customer"
  }
]
```

#### `GET /api/Appointments/{id}`
**Description:** Get an appointment by ID.

**Parameters:**
- `id`: Appointment ID (path)

**Responses:**
- `200 OK`
- `404 Not Found`

#### `POST /api/Appointments`
**Description:** Create a new appointment.

**Request Body:**
```json
{
  "customerId": 1,
  "serviceId": 1,
  "startTime": "2024-03-20T10:00:00",
  "endTime": "2024-03-20T11:00:00",
  "notes": "First time customer"
}
```

**Responses:**
- `201 Created`
- `400 Bad Request`

#### `PUT /api/Appointments/{id}`
**Description:** Update an existing appointment.

**Parameters:**
- `id`: Appointment ID (path)

**Responses:**
- `204 No Content`
- `400 Bad Request`
- `404 Not Found`

#### `DELETE /api/Appointments/{id}`
**Description:** Cancel/delete an appointment.

**Parameters:**
- `id`: Appointment ID (path)

**Responses:**
- `204 No Content`
- `404 Not Found`

### Packages

#### `GET /api/Packages`
**Description:** Get all service packages.

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Spa Day Package",
    "description": "Complete spa treatment package",
    "services": [
      {
        "id": 1,
        "title": "Massage"
      }
    ]
  }
]
```

#### `GET /api/Packages/{id}`
**Description:** Get a package by ID.

**Parameters:**
- `id`: Package ID (path)

**Responses:**
- `200 OK`
- `404 Not Found`

#### `POST /api/Packages`
**Description:** Create a new package.

**Request Body:**
```json
{
  "title": "Spa Day Package",
  "description": "Complete spa treatment package",
  "serviceIds": [1, 2, 3]
}
```

**Responses:**
- `201 Created`
- `400 Bad Request`

#### `PUT /api/Packages/{id}`
**Description:** Update an existing package.

**Parameters:**
- `id`: Package ID (path)

**Responses:**
- `204 No Content`
- `400 Bad Request`
- `404 Not Found`

#### `DELETE /api/Packages/{id}`
**Description:** Delete a package.

**Parameters:**
- `id`: Package ID (path)

**Responses:**
- `204 No Content`
- `404 Not Found`