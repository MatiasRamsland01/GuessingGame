FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY /GuessingGame /GuessingGame

WORKDIR "/GuessingGame"


RUN apt-get -y update && apt-get -y upgrade

RUN dotnet dev-certs https

RUN dotnet tool install --global dotnet-ef




ENV PATH $PATH:/root/.dotnet/tools

RUN dotnet ef database update
RUN dotnet restore
RUN dotnet build

ENTRYPOINT ["dotnet", "run", "--launch-profile", "Release"]
