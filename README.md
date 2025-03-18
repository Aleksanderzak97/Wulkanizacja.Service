# Wulkanizacja Service

## Opis
Wulkanizacja Service to serwis webowy stworzony w technologii .NET 8, który umożliwia zarządzanie oponami w serwisie wulkanizacyjnym. API pozwala na dodawanie, aktualizowanie, usuwanie oraz pobieranie informacji o oponach.

## Funkcje
- Dodawanie nowej opony
- Pobieranie listy opon na podstawie rozmiaru i typu
- Pobieranie danych konkretnej opony na podstawie identyfikatora
- Aktualizowanie danych opony
- Usuwanie opony
- Walidacja tokenów JWT

## Wymagania
- .NET 8 SDK
- PostgreSQL
- Wulkanizacja.Auth

## Konfiguracja
Plik `appsettings.json` zawiera konfigurację połączenia z bazą danych PostgreSQL:
```json
 { "postgres": { "ConnectionString": "Database=wulkanizacja_db;Username=postgres;Enlist=False;Password=admin;Port=5432;Host=localhost;TimeZone=Europe/Warsaw" } }
```
W przypadku dockera konfiguracja w docker-compose.yml


## Uruchomienie
1. Uruchom instancje serwisu Wulkanizacja.Auth
2. Sklonuj repozytorium.
3. Upewnij się, że masz zainstalowane .NET 8 SDK.
4. Skonfiguruj bazę danych PostgreSQL zgodnie z ustawieniami w `appsettings.json`.
5. Uruchom aplikację za pomocą Visual Studio 2022.
6. Migracje zostaną zastosowane automatycznie podczas uruchamiania aplikacji.
7. Do testów za pomocą API swaggera trzeba się zautoryzowac klikając authorize i wpisując token który załączam w pliku Jwt token.json czyli Bearer 'token'
7. Teraz można testować endpointy z załączonej kolekcji postmana lub przez API swaggera pod adresem [http://localhost:5884/swagger](http://localhost:5884/swagger).

## Uruchomienie Alternatywne Docker
1. Zainstaluj program Docker Desktop
2. Wejdz do folderu Wulkanizacja.Service
3. Skonfiguruj bazę danych PostgreSQL zgodnie z ustawieniami w `appsettings.json`. plik znajduję się w folderze Wulkanizacja.Service.Api
4. Wróć do poprzedniego folderu czyli Wulkanizacja.Service uruchom cmd z tego poziomu
5. Wpisz docker-compose build a następnie docker-compose up -d
6. Do testów za pomocą API swaggera trzeba się zautoryzowac klikając authorize i wpisując token który załączam w pliku Jwt token.json czyli Bearer 'token'
7. Teraz można testować endpointy z załączonej kolekcji postmana lub przez API swaggera pod adresem [http://localhost:5884/swagger](http://localhost:5884/swagger).


## Endpointy
- `POST    /tires` - Dodaje nową oponę.
- `GET     /tires/size/{Size}/TireType/{TireType}` - Pobiera listę opon o podanym rozmiarze i typie.
- `GET     /tires/{TireId}` - Pobiera dane konkretnej opony.
- `PUT     /tires/updateTire/{TireId}` - Aktualizuje dane opony.
- `DELETE  /tires/{TireId}/removeTire` - Usuwa oponę.

## Dokumentacja API
Dokumentacja API jest dostępna pod adresem [http://localhost:5884/swagger](http://localhost:5884/swagger) po uruchomieniu aplikacji.
Aby przetestować endpointy, można użyć załączonej kolekcji Postmana znajdującej się w katalogu `Postman` (Tutaj gdzie ten README)

## Autorzy
- [Aleksander Żak]
