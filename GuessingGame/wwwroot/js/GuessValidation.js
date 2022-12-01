// Functionality for Event that on click checks for empty guess string
document.getElementById('UserGuessButton').addEventListener("click", IsEmpty);
document.getElementById('UserGuess').addEventListener("focus", Empty);

function IsEmpty(event) {

    if (UserGuess.style.color == "red" || UserGuess.value.trim().length === 0) {
        UserGuess.style.color = "red";
        UserGuess.value = "Guess can not be empty!";
        event.preventDefault();
    }
}

function Empty(event) {
    if (UserGuess.style.color == "red") {
        UserGuess.style.color = "black";
        UserGuess.value = ""
    }
    event.preventDefault();
}
