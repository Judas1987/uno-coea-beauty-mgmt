# Beauty Salon Management System - Senior Developer Challenge

## 🎯 Core Requirements

### 1. Appointment Service (C#)
- Implement CRUD operations for appointments
- Handle scheduling conflicts
- Integrate with notification service
- Include unit tests (xUnit/NUnit)

### 2. Customer Service (C#)
- Manage customer profiles
- Track appointment history
- Implement loyalty points system
- Add search functionality

### 3. Service Catalog (C#)
- Maintain service inventory
- Manage pricing and packages
- Associate services with categories
- Handle seasonal promotions

### 4. Angular Admin Portal
- Responsive dashboard with:
  - Calendar view (FullCalendar integration)
  - Customer management CRUD
  - Service configuration
  - Basic reporting

## 🔧 Setup Instructions

1. **Prerequisites**:
   - .NET 6 SDK
   - Node.js 16+
   - Docker Desktop
   - SQL Server/PostgreSQL

2. **Running the system**:
   ```bash
   docker-compose up -d
   cd src/angular-admin
   npm install
   ng serve

3. **Access endpoints:**
API Gateway: http://localhost:5000
Angular App: http://localhost:4200

🧪 Testing Approach
Unit Tests: 70%+ coverage per service
Integration Tests: API endpoint validation
E2E Tests: Cypress for Angular components
Load Testing: Locust for performance validation

📄 **Documentation Expectations**
**Code-Level**:
XML documentation for public APIs
Clear method comments for complex logic

**System-Level**:
Architecture diagram
API contracts (OpenAPI/Swagger)

🏆 **Evaluation Criteria**
Category	Weight
Code Quality	25%
Architecture	25%
Functionality	20%
Testing	15%
Documentation	10%
DevOps Setup	5%

💡 **Bonus Features (Optional)**
Real-time updates with SignalR
Customer self-booking portal
Payment integration
CI/CD pipeline example
Performance monitoring
