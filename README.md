1) After cloning change string-connection to PostgreSQL in TranslationService.Grpc/appsettings.json.
2) Open project in terminal and execute:
  dotnet ef migrations add InitialCreate
  dotnet ef database update
3) Now you can build project
