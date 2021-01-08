#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BookMySlot/BookMySlot/Bookmyslot.Api/Bookmyslot.Api.csproj", "BookMySlot/BookMySlot/Bookmyslot.Api/"]
RUN dotnet restore "BookMySlot/BookMySlot/Bookmyslot.Api/Bookmyslot.Api.csproj"
COPY . .
WORKDIR "/src/BookMySlot/BookMySlot/Bookmyslot.Api"
RUN dotnet build "Bookmyslot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookmyslot.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookmyslot.Api.dll"]