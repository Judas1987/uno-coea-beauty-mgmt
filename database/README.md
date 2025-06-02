# BeautyManagement

### Entity Relationship Diagram

```mermaid
erDiagram

Customers {
    int Id
    string FirstName
    string LastName
    string Email
    string PhoneNumber
    int LoyaltyPoints
}

Appointments {
    int Id
    int CustomerId
    int ServiceId
    datetime StartTime
    datetime EndTime
    string Status
    string Notes
}

Services {
    int Id
    string Title
    string Description
    decimal Price
    int DurationMinutes
    int CategoryId
    boolean IsActive
    boolean IsPromotional
    decimal PromotionalPrice
}

ServiceCategories {
    int Id
    string Title
    string Description
}

Packages {
    int Id
    string Title
    string Description
}

ServicePackages {
    int PackageId
    int ServiceId
}

Customers ||--o{ Appointments : books
Appointments }o--|| Services : includes
Services }o--|| ServiceCategories : belongs_to
Packages ||--o{ ServicePackages : groups
Services ||--o{ ServicePackages : part_of
```