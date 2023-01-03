FROM mcr.microsoft.com/dotnet/runtime:6.0
COPY . /app
WORKDIR /app
RUN dotnet restore
RUN dotnet build
RUN dotnet add package Microsoft.Data.SqlClient
EXPOSE 80
ENTRYPOINT ["dotnet", "myapi.dll"]