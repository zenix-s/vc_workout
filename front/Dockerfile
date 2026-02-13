# Build Tailwind CSS stage
FROM node:20-alpine AS css-build
WORKDIR /src
COPY front/vc_workout.wasm/package*.json ./
RUN npm install
COPY front/vc_workout.wasm/Styles ./Styles
RUN npm run build:css

# Build .NET stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file
COPY front/vc_workout.wasm/*.csproj ./front/vc_workout.wasm/

# Restore dependencies
RUN dotnet restore ./front/vc_workout.wasm/vc_workout.wasm.csproj

# Copy all source code
COPY front/vc_workout.wasm/ ./front/vc_workout.wasm/

# Copy built CSS from css-build stage
COPY --from=css-build /src/wwwroot/css/app.css ./front/vc_workout.wasm/wwwroot/css/app.css

# Publish the application
RUN dotnet publish ./front/vc_workout.wasm/vc_workout.wasm.csproj -c Release -o /app/publish

# Runtime stage with nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy published Blazor files
COPY --from=build /app/publish/wwwroot .

# Copy custom nginx configuration
COPY front/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
