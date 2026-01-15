# Garage 3.0

## Hantrera en garage som tar hand om olika fordon

### Första gång Run:
```bash
dotnet ef update-database
dotnet run Garaage
```

### Sedan kör bara:
```bash
dotnet run Garaage
```

Applikotionen innehåler lokalt database med default fordon, roles och 2 test users.

#### Test Users:
| name    | Username | Password |
| -------- | ------- |
| **Admin Adminsson ** | admin@Garage.se | Admin123! |
| **Member Membersson** | member@Garage.se | Member123! |

Det går att registrera sig som ny användare också.