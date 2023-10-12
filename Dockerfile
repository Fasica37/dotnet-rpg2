# Use the official .NET 7 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory in the container
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY *.csproj ./
RUN dotnet restore

# Copy the project files into the container
COPY . ./
RUN dotnet publish -c Release -o out

# Build the application
RUN dotnet publish -c Release -o out

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/out .

# Start the application
ENTRYPOINT ["dotnet", "dotnet-rpg2.dll"]
