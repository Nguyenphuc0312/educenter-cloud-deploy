FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["StudentAttendanceService.csproj", "./"]
RUN dotnet restore "StudentAttendanceService.csproj"

COPY . .
RUN dotnet publish "StudentAttendanceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5002

ENV ASPNETCORE_URLS=http://+:5002

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StudentAttendanceService.dll"]
