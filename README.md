# Readme
A Guessing Game application where the goal is to collect what humans think are important to identify objects in the image. So a game with a purpose (GWAP)

## Running the application
To run the application you use docker with the following command:
```linux
sudo docker compose up
```

## File structure
GuessingGame (main application)
-  Infrastructure
    -  Include the DB-context
    -  Migrations
    -  Images
-  Core/Domain
    -  Oracle domain
    -  User domain
    -  Images domain
    -  History domain
-  Pages
    -  Account
      -  Login
      -  Register
      -  Logout
    -  User
      -  Index (authorized)
      -  Upload image
      -  Game view
    -  Index page
    -  Error page
GuessingGame.Tests
- Historytest
- Imagetests
- Oracle

