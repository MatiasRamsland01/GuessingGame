
// This jQuery sends the x and y coordinates of a user click
// to propose better segments for AI
$('#coverDiv').on('click', function (event) {
    var pxx = event.offsetX;
    var pxy = event.offsetY;
    console.log("Jquery")

    var imageId = document.getElementById('ImageId').value;
    imageId = parseInt(imageId);

    var message = {
        xCoord: pxx,
        yCoord: pxy,
        ImageId: imageId,
    };

    $.ajax({
        url: '/User/GameView',
        type: 'POST',
        dataType: 'json',
        headers: {
            RequestVerificationToken: document.getElementById('CSRF').value,
        },
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(message),
        success: function (data) {
            console.log("Success")
            window.location.href = data;
        },
        failure: function (data) {
            console.log("Failure")
            var f = data;
        },
    });
});
//get first image slice to get size
var coverDiv = document.getElementById('coverDiv');
var firstImage = document.getElementById('firstImg');
//set size of div to cover full image
coverDiv.style.height = firstImage.height + 'px';
coverDiv.style.width = firstImage.width + 'px';
coverDiv.style.backgroundColor = 'rgba(72, 122, 180, 0)';




