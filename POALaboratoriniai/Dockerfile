FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["POALaboratoriniai.csproj", "./"]
RUN dotnet restore "POALaboratoriniai.csproj"

COPY . .
RUN dotnet publish "POALaboratoriniai.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "POALaboratoriniai.dll"]
