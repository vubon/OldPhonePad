# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy all files
COPY . .

# Build only the console application
RUN dotnet build OldPhonePad.Console/OldPhonePad.Console.csproj --configuration Release --no-restore

# Use the official .NET 9.0 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/OldPhonePad.Console/bin/Release/net9.0/ ./

# Set the entry point
ENTRYPOINT ["dotnet", "OldPhonePad.Console.dll"]
