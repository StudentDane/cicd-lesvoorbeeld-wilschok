# build stage 
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env
WORKDIR /app

# restore stage
# copy csproj files and restore packages - separate container layers
# starting from solution is also possible
COPY CICD.Lib/*.csproj ./CICD.Lib/
RUN dotnet restore ./CICD.Lib/*.csproj
COPY CICD.Test/*.csproj ./CICD.Test/
RUN dotnet restore ./CICD.Test/*.csproj
COPY CICD.Web/*.csproj ./CICD.Web/
RUN dotnet restore ./CICD.Web/*.csproj

# copy source code
COPY . . 

# test stage
# run separate: cached layer if tests success
ENV TEAMCITY_PROJECT_NAME = ${TEAMCITY_PROJECT_NAME}
RUN dotnet test CICD.Test/CICD.Test.csproj --verbosity=normal

# build in release mode to folder publish
RUN dotnet publish CICD.Web/CICD.Web.csproj -c Release -o /publish 

# runtime image stage
FROM mcr.microsoft.com/dotnet/core/sdk:2.2
# put release build-files in runtime image /publish
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT [ "dotnet","CICD.Web.dll" ]
