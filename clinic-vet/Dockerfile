#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["clinic-vet/clinic-vet.csproj", "clinic-vet/"]
RUN dotnet restore "clinic-vet/clinic-vet.csproj"
COPY . .
WORKDIR "/src/clinic-vet"
RUN dotnet build "clinic-vet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "clinic-vet.csproj" -c Release -o /app/publish 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "clinic-vet.dll"]