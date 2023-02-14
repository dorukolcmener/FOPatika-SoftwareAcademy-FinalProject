classDiagram
    class Admin {
        Edit(User usr)
        Edit(Apartment apt)
        AssignBill(Bill bill,User)
    }

    class User {
        string Name
        string Surname
        string EMail
        string Phone
        int TCNo

        void SendMessage(string Msg)
        void Pay(Bill bill)
    }

    class Apartment {
        string Block
        int Floor
        int FlatNo
        string FlatType
        User Owner
        User? Renter
    }

    class Vehicle {
        string LicensePlate
    }

    class Bill {
        int Amount
        string Type
    }

    Admin -- User
    Admin --|> User
    Admin --> Apartment
    Admin --> Bill
    
    User "1" *-- "*" Vehicle
    User "1" *-- "*" Apartment
    Apartment "1" *-- "*" Bill