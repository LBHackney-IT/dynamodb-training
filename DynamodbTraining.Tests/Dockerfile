FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./DynamodbTraining.sln ./
COPY ./DynamodbTraining/DynamodbTraining.csproj ./DynamodbTraining/
COPY ./DynamodbTraining.Tests/DynamodbTraining.Tests.csproj ./DynamodbTraining.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./DynamodbTraining/DynamodbTraining.csproj
RUN dotnet restore ./DynamodbTraining.Tests/DynamodbTraining.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out DynamodbTraining.Tests/DynamodbTraining.Tests.csproj

CMD dotnet test