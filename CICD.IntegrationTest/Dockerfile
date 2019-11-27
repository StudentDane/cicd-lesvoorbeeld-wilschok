FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /integration

COPY CICD.IntegrationTest.csproj .
RUN dotnet restore 

COPY . . 
ENTRYPOINT [ "dotnet", "test" ]

