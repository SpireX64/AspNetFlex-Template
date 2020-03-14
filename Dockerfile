FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# Copy sources
COPY Estimate.WebApp/ Estimate.WebApp/
COPY Estimate.Api/ Estimate.Api/
COPY Estimate.Domain/ Estimate.Domain/
COPY Estimate.Data/ Estimate.Data/
COPY Estimate.DatabaseStorage/ Estimate.DatabaseStorage/
COPY libs/*.dll libs/

# Build
WORKDIR /src/Estimate.WebApp
RUN dotnet build "Estimate.WebApp.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Estimate.WebApp.csproj" -c Release -o /app/publish

# Finalize image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app/publish
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Estimate.WebApp.dll"]
EXPOSE 80
EXPOSE 443