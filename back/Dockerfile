# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY back/vc_workout.slnx ./back/
COPY back/vc_workout.api/*.csproj ./back/vc_workout.api/
COPY back/vc_workout.infraestructure/*.csproj ./back/vc_workout.infraestructure/
COPY back/vc_workout.shared/*.csproj ./back/vc_workout.shared/

# Restore dependencies
RUN dotnet restore ./back/vc_workout.api/vc_workout.api.csproj

# Copy all source code
COPY back/ ./back/

# Build the application
RUN dotnet build ./back/vc_workout.api/vc_workout.api.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish ./back/vc_workout.api/vc_workout.api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copy published files
COPY --from=publish /app/publish .

# Create directory for SQLite database
RUN mkdir -p /app/data

ENTRYPOINT ["dotnet", "vc_workout.api.dll"]
