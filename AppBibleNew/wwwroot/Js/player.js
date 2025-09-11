window.audioPlayer = {
    play: () => {
        var audio = document.getElementById("player");
        if (audio) audio.play();
    },
    pause: () => {
        var audio = document.getElementById("player");
        if (audio) audio.pause();
    },
    stop: () => {
        var audio = document.getElementById("player");
        if (audio) {
            audio.pause();
            audio.currentTime = 0;
        }
    },
    setVolume: (v) => {
        var audio = document.getElementById("player");
        if (audio) audio.volume = v;
    }
};
