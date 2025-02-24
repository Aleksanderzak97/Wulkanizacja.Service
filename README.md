# Wulkanizacja Service API

## Opis
Wulkanizacja Service API to aplikacja webowa stworzona w technologii .NET 8, która umożliwia zarządzanie oponami w serwisie wulkanizacyjnym. API pozwala na dodawanie, aktualizowanie, usuwanie oraz pobieranie informacji o oponach.

## Funkcje
- Dodawanie nowej opony
- Pobieranie listy opon na podstawie rozmiaru i typu
- Pobieranie danych konkretnej opony na podstawie identyfikatora
- Aktualizowanie danych opony
- Usuwanie opony

## Wymagania
- .NET 8 SDK
- PostgreSQL

## Konfiguracja
Plik `appsettings.json` zawiera konfigurację połączenia z bazą danych PostgreSQL:

{ "postgres": { "ConnectionString": "Database=wulkanizacja_db;Username=postgres;Enlist=False;Password=admin;Port=5432;Host=localhost;TimeZone=Europe/Warsaw" } }


## Uruchomienie
1. Sklonuj repozytorium.
2. Upewnij się, że masz zainstalowane .NET 8 SDK.
3. Skonfiguruj bazę danych PostgreSQL zgodnie z ustawieniami w `appsettings.json`.
4. Uruchom aplikację za pomocą Visual Studio 2022.
5. Migracje zostaną zastosowane automatycznie podczas uruchamiania aplikacji.
6. Teraz można testować endpointy z załączonej kolekcji postmana lub przez API swaggera pod adresem [http://localhost:5884/swagger](http://localhost:5884/swagger).


## Endpointy
- `POST /tires` - Dodaje nową oponę.
- `GET /tires/size/{Size}/TireType/{TireType}` - Pobiera listę opon o podanym rozmiarze i typie.
- `GET /tires/{TireId}` - Pobiera dane konkretnej opony.
- `PUT /tires/updateTire/{TireId}` - Aktualizuje dane opony.
- `DELETE /tires/{TireId}/removeTire` - Usuwa oponę.

## Dokumentacja API
Dokumentacja API jest dostępna pod adresem `[http://localhost:5884/swagger](http://localhost:5884/swagger)` po uruchomieniu aplikacji.

## Autorzy
- [Aleksander Żak]
