FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

WORKDIR /app
EXPOSE 5000


ARG DIR_OF_FILES
ARG ALLOW_ANONYMOUS_LOGIN
ENV DIR_OF_FILES $DIR_OF_FILES
ENV ALLOW_ANONYMOUS_LOGIN $ALLOW_ANONYMOUS_LOGIN

# copy csproj and restore as distinct layers
COPY ${DIR_OF_FILES}*.sln .
COPY ${DIR_OF_FILES}*.csproj .


RUN dotnet restore

# copy everything else and build app
COPY ${DIR_OF_FILES} ./ConsoleApp13/
WORKDIR /app/ConsoleApp13

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime


WORKDIR /app
COPY --from=build /app/ConsoleApp13/out .
ENTRYPOINT ["dotnet", "ConsoleApp13.dll"]

