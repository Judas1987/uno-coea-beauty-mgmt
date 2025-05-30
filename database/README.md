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
    datetime DateOfBirth
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
    string Name
    string Description
    decimal Price
    int DurationMinutes
    int CategoryId
    boolean IsActive
    boolean IsPromotional
    decimal PromotionPrice
}

ServiceCategories {
    int Id
    string Name
    string Description
}

ServicePackages {
    int Id
    string Name
    string Description
    decimal Price
    datetime SeasonStart
    datetime SeasonEnd
    boolean IsActive
}

PackageServices {
    int PackageId
    int ServiceId
}

Customers ||--o{ Appointments : has
Appointments }o--|| Services : for
Services }o--|| ServiceCategories : categorized
ServicePackages ||--o{ PackageServices : includes
Services ||--o{ PackageServices : part_of
```