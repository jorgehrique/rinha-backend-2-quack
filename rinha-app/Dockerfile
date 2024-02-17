FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

# Copy everything
COPY . ./

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "rinha-app.dll"]

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# # Install NativeAOT build prerequisites
# RUN apt-get update \
#     && apt-get install -y --no-install-recommends \
#        clang zlib1g-dev

# WORKDIR /source

# COPY . .
# RUN dotnet publish -o /app rinha-app.csproj

# FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
# WORKDIR /app
# COPY --from=build /app .
# ENTRYPOINT ["/app/rinha-app"]