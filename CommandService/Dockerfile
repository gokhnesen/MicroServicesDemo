FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CommandService/CommandsService.csproj", "CommandService/"]
RUN dotnet restore "CommandService/CommandsService.csproj"

COPY CommandService/ CommandService/
RUN dotnet publish "CommandService/CommandsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CommandsService.dll"]
