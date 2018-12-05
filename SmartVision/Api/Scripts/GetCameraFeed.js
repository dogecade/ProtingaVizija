const videoSelect = document.querySelector("select#videoSource");
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