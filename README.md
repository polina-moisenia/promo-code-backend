# Promo Code Backend

ASP.NET Core backend for the Promo Code Management system. It provides two SignalR hubs:

- **PromoCodeGenerationHub**: For generating promo codes.
- **PromoCodeUsageHub**: For activating promo codes.

## Independent Use

**Prerequisites**:
- .NET 9 SDK
- Docker installed for managing the database

1. Create the required Docker volume:
   ```bash
   docker volume create db_data
   ```
2. Start the database:
   ```bash
   docker-compose -f db-docker-compose.yml up -d
   ```
3. Restore dependencies and run the backend service:
   ```bash
   cd BE/promo-code-backend
   dotnet restore
   dotnet run
   ```

Accessible at:
- `http://localhost:5184/promoCodeHub`
- `http://localhost:5184/promoCodeUsageHub`
