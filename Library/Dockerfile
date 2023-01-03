FROM mcr.microsoft.com/dotnet/sdk:6.0

# Copy the project files
COPY . /Library

# Change to the app directory
WORKDIR /Library

# Restore dependencies
RUN dotnet restore

# Build the project
RUN dotnet build

# Expose port 7211
EXPOSE 7211