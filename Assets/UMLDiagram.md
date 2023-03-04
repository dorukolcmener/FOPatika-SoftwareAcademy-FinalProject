classDiagram
    class Admin {
        View(Users, Apartments, Bills)
        Create(...)
        Update/Edit(...)
        Delete(...)
    }

    class User {
        string Name
        string Surname
        string EMail
        string Phone
        int TCNo
        Enum Role Admin|Owner|Renter

        void SendMessage(string Msg)
        void Pay(Bill bill)
    }

    class Apartment {
        string Block
        int Floor
        int Door
        string FlatType
        User Resident
    }

    class Vehicle {
        string LicensePlate
        User Owner
    }

    class Bill {
        int Amount
        Enum Type
    }

    User -- Admin
    User <|-- Admin
    Admin --> Apartment
    Admin --> Bill
    
    User "1" *-- "*" Vehicle
    User "1" *-- "*" Apartment
    Apartment "1" *-- "*" Bill