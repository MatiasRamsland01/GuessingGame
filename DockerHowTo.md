# What is Docker?

Look at a Docker container as a virtual machine running on it's own.
It is not strong enough to run many programs, but we use different Docker-Containers to run individual programs.
We are going to use one container to run our app.

# Install Docker and Docker-compose

Install docker and docker-compose in the same way you install any other program.

In Linux Debian/Ubuntu distributions it would be 

```sudo apt install docker```
and
```sudo apt install docker-compose```

For another/your OS you could get more information about installing it on the docker website.




# The Dockerfile explained

### Copying the project from your local file to the docker container.
    FROM mcr.microsoft.com/dotnet/sdk:6.0
    COPY /GuessingGame /GuessingGame

### The directory inside the container the program is located
    WORKDIR "/GuessingGame"

### Since docker container runs a linux debian distribution the container kernel should be upgraded.
    RUN apt-get -y update && apt-get -y upgrade

### To run https inside the container. Was thinking the program.cs file would do that but it does not so.. we need it here.
    RUN dotnet dev-certs https

### Installs the dotnet ef core package inside the container.
    RUN dotnet tool install --global dotnet-ef

### dotnet tools path
    ENV PATH $PATH:/root/.dotnet/tools


### Running dotnet restore and build inside the container.
    RUN dotnet restore
    RUN dotnet build

### Tells docker what command to run to start the app
    ENTRYPOINT ["dotnet", "run"]



# The Docker-compose.yml file explained

### Version and services which we may use later
    version: "3.8"
    services:
    web:

### Build from current directory
    build: "./"

### Maps your localhost ip and port number (127.0.0.1:5001) to the container which is listening to 0.0.0.0:5000.
    ports:
        - "127.0.0.1:5001:5000"



# Build the docker container

You should now be able to run ```docker-compose up --build``` in your terminal while inside the ramriibaermelpedlar/project folder.

The --build will build your container containing the docker image.

You will only need to build it once, afterwards you only need to run ```docker-compose up```.

You can also run ```docker-compose up --build -d``` with the -d.
The -d option detaches the container and makes it run in the background if you want it to.

See more about the options to docker-compose [here](https://docs.docker.com/engine/reference/commandline/compose_up/).

When docker is running you will see a similar output in the terminal as when we run the app with ```dotnet run```.
However, you will need to go to your browser and go to localhost:5001 or 127.0.0.1:5001 to get to the app.

The IP in the output is the docker listing port.

When the container is running you can open another terminal and run ```docker ps```, this will show you all the containers running.

To stop a running container run ```docker stop``` with the docker ID shown to the far left when running ```docker ps```.

To "cleanse" the containers - if you did something wrong run ```docker system prune```, this will delete/remove all containers.



# Thats all folks!

I think I covered most of the process to run docker, do not hesitate to ask if something is unclear :).


-Roger