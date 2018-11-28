const videoSelect = document.querySelector("select#videoSource");
const canvas = document.querySelector("select#canvas");
const constraints = {
    video: {
        facingMode: "environment"
    }
}

function gotStream(stream) {
    window.stream = stream; // make stream available to console
    video.srcObject = stream;
}

function handleError(error) {
    console.log("navigator.getUserMedia error: ", error);
}

function start() {
    navigator.mediaDevices.getUserMedia(constraints).then(gotStream).catch(handleError);
}

function stop() {
    video.pause();
    video.src = "";
    stream.getTracks().forEach(track => track.stop());
    console.log("Stopping stream");
}

function snapshot() {
    console.log(canvas);
    canvas.width = 200;
    canvas.height = 200;
    canvas.getContext('2d').drawImage(video, 0, 0, canvas.width, canvas.height);
    var img = canvas.toDataURL();
    $.ajax({
        url: '/CameraStream/CaptureSnapshot',
        type: 'POST',
        dataType: "json",
        data: { 'imgBase64': JSON.stringify(img) },
        success: function (data) {
            alert(data.result);
        },
        error: function () {
            alert("nixuj");
        }
    });
}