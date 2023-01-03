FROM mcr.microsoft.com/dotnet/sdk:5.0

COPY . /Library

WORKDIR /Library
RUN dotnet restore
RUN dotnet build
