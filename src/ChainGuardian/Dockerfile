# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ChainGuardian.csproj", "./"]
RUN dotnet restore "ChainGuardian.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ChainGuardian.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChainGuardian.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChainGuardian.dll"]
