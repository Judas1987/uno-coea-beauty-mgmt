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

### Customers

### `GET /customers`

**Description:** Get all customers.

**Responses:**
- `200 OK`
- `404 Not Found`

**Response Example:**
```json
[
  {
    "id": 1,
    "userName": "moviebuff88",
    "email": "buff@example.com",
    "isAdmin": false,
    "isDeleted": false
  },
  {
    "id": 2,
    "userName": "admin_user",
    "email": "admin@example.com",
    "isAdmin": true,
    "isDeleted": false
  }
]
```

### Appointments

### Packages