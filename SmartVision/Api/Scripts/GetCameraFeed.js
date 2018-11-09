async function StartStreamCapturing() {
    var button = document.getElementById("enableStream");

    if (navigator.mediaDevices.getUserMedia !== null) {
        var options = {
            video: true,
            audio: false
        };

        if (button.innerText == "Enable camera") {
            navigator.webkitGetUserMedia(options,
                function (stream) {
                    video.src = window.URL.createObjectURL(stream);
                    window.stream = stream;
                    video.play();
                    console.log("Starting stream");

                    button.innerText = "Disable camera";
                }, function (e) {
                    console.log("background error : " + e.name);
                });
        } else {
            video.pause();
            video.src = "";
            stream.getTracks().forEach(track => track.stop());
            console.log("Stopping stream");

            button.innerText = "Enable camera";
        }
    }
}