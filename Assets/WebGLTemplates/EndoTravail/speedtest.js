var imageAddr = "https://static.dianaportela.fr/speedtest.jpg";
var downloadSize = 5552389; // Content-Length header response value
var bytesInAKilobyte = 1024;
var roundedDecimals = 2;
var download = new Image();
var iterations = 5;
var startTime, endTime;
var speeds = [];
var averageSpeed;

function speedTestAsync() {
  return new Promise((resolve, reject) => {
    runTest(iterations, resolve, reject);
  });
}

function runTest(iteration, resolve, reject) {
    imageAddr += "?n=" + Math.random();
    download.onload = function () {
        endTime = (new Date()).getTime();
        showResults();

        if (iteration > 1) {
            setTimeout(function () {
                runTest(iteration - 1, resolve, reject);
            }, 500);
        } else {
            displayAverageSpeed();
            resolve(averageSpeed);
        }
    }

    download.onerror = function() {
      document.getElementById("unity-progress-bar-empty").background = "none";
      document.getElementById("loading-text").innerHTML = "Il vous faut une connection internet pour jouer à ce jeu.";
    };

    startTime = (new Date()).getTime();
    download.src = imageAddr;
}

function showResults() {
    var duration = (endTime - startTime) / 1000;
    var bitsLoaded = downloadSize * 8 * 8;
    var speedBps = (bitsLoaded / duration).toFixed(roundedDecimals);
    speeds.push(parseFloat(speedBps));

    displayCurrentSpeed(speedBps);
}

function displayCurrentSpeed(speedBps) {
    var displaySpeed = speed(speedBps);
    var currentSpeed = displaySpeed.value + " " + displaySpeed.units;
    //console.log("Current Speed:", currentSpeed);
    document.getElementById("speed-list").innerText += currentSpeed + "\n";
}

function displayAverageSpeed() {
    var totalSpeed = speeds.reduce((acc, curr) => acc + curr, 0);
    var averageSpeedBps = (totalSpeed / speeds.length).toFixed(roundedDecimals);
    averageSpeed = speed(averageSpeedBps);
    //console.log("Average Speed:", averageSpeed.value + " " + averageSpeed.units);
    document.getElementById("average-speed").innerText += averageSpeed.value + " " + averageSpeed.units;
}

function speed(bitsPerSecond) {
    var KBps = (bitsPerSecond / bytesInAKilobyte).toFixed(roundedDecimals);
    if (KBps <= 1) return { value: bitsPerSecond, units: "Bps" };
    var MBps = (KBps / bytesInAKilobyte).toFixed(roundedDecimals);
    if (MBps <= 1) return { value: KBps, units: "KBps" };
    else return { value: MBps, units: "MBps" };
}
