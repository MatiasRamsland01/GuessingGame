const checkbox = document.getElementById("manualSlice");
var canvas;
var box;
var image;
var range;
var label;
var containerRange;
var colours = [];

function clearImage() {
    console.log("inside clear image");
    canvas = document.getElementById('canvas');
    let context = canvas.getContext('2d');
    colours = []
    context.clearRect(0, 0, canvas.width, canvas.height);

}

checkbox.addEventListener('click', function handleClick() {
    containerRange = document.getElementById("containerRange");
    box = document.getElementById('container');
    range = document.getElementById("penWidthRange")
    label = document.getElementById("labelPenWidth")
    image = document.getElementById("showImage");
    canvas = document.getElementById('canvas');
    console.log("Checkbox pressed")


    if (checkbox.checked) {
        var image = document.getElementById('showImage');
        if (image.src != "") {
            containerRange.style.display = "block"
            box.style.display = 'block';
            canvas.style.display = "block"
            image.style.display = "block"
            let width = image.width
            let height = image.height
            if (width > 500 || height > 500) {
                var canvasResize = document.createElement('canvas'),
                    contextResize = canvasResize.getContext('2d');
                canvasResize.width = 400;
                canvasResize.height = 400 * (height / width);
                width = canvasResize.width
                height = canvasResize.height

                contextResize.drawImage(image, 0, 0, canvasResize.width, canvasResize.height);
                image.src = canvasResize.toDataURL();
                document.getElementById("image").value
            }

            var rect = document.getElementById("container").getBoundingClientRect();
            container.style.width = width + "px";
            container.style.height = height + "px";
            if (isCanvasBlank(canvas)) {
                canvas.width = width
                canvas.height = height
                canvas.style.zIndex = 2
                canvas.style.top = rect.top + "px"
                canvas.style.left = rect.left + "px"
                ctx = canvas.getContext('2d');
                ctx.lineWidth = range.value;
                canvas.onmousedown = startDrawing;
                canvas.onmouseup = stopDrawing;
                canvas.onmousemove = draw;
            }

        }
    }
    else {
        containerRange.style.display = "none"
        box.style.display = 'none';
    }
})

var loadFile = function (event) {
    var image = document.getElementById('showImage');
    image.src = URL.createObjectURL(event.target.files[0]), 0, 0
    //Resets checkbox if. Fixes bug if error message occur
    document.getElementById("manualSlice").checked = false;

}


function startDrawing(e) {
    const red = Math.floor(Math.random() * 255).toString();
    const green = Math.floor(Math.random() * 255).toString();
    const blue = Math.floor(Math.random() * 255).toString();
    ctx.strokeStyle = `rgba(${red}, ${green}, ${blue}, 1)`;
    document.getElementById("Colors").value += `${red},${green},${blue},255,`
    isDrawing = true;
    ctx.beginPath();
    ctx.moveTo(e.pageX - canvas.offsetLeft, e.pageY - canvas.offsetTop);
}

function draw(e) {
    if (isDrawing == true) {
        var x = e.pageX - canvas.offsetLeft;
        var y = e.pageY - canvas.offsetTop;
        ctx.arc(x, y, range.value, 0, 2 * Math.PI);
        //ctx.lineTo(x, y);
        ctx.stroke();
    }
}

function isCanvasBlank(canvas) {
    return !canvas.getContext('2d')
        .getImageData(0, 0, canvas.width, canvas.height).data
        .some(channel => channel !== 0);
}

function stopDrawing() {
    isDrawing = false;
    document.getElementById("SlicedImage").value = canvas.toDataURL()
}
