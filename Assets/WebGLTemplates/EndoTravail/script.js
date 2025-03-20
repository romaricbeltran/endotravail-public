var container = document.querySelector("#unity-container");
var canvas = document.querySelector("#unity-canvas");
var loadingBar = document.querySelector("#unity-loading-bar");
var progressBarFull = document.querySelector("#unity-progress-bar-full");
var loadingText = document.getElementById("loading-text");
var warningBanner = document.querySelector("#unity-warning");

var fullscreenButton = document.querySelector("#unity-fullscreen-overlay");
fullscreenButton.classList.add("disabled");

function unityShowBanner(msg, type) {
    var div = document.createElement('div');
    div.innerHTML = msg;
    warningBanner.appendChild(div);
    if (type == 'error') div.style = 'background: red; padding: 10px;';
    else if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
    warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
}

var buildUrl = "Build";
var loaderUrl = buildUrl + "/debugendo.dianaportela.fr.loader.js";
var config = {
    dataUrl: buildUrl + ((/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) ?
    "/debugendo.dianaportela.fr.data.gz" : "/debugendo.dianaportela.fr.data.gz"),
    frameworkUrl: buildUrl + "/debugendo.dianaportela.fr.framework.js.gz",
    codeUrl: buildUrl + "/debugendo.dianaportela.fr.wasm.gz",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "RomaricB",
    productName: "seriousgame-endometriose-travail.dianaportela.fr",
    productVersion: "1",
    showBanner: unityShowBanner,
};

if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    var meta = document.createElement('meta');
    meta.name = 'viewport';
    meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
    document.getElementsByTagName('head')[0].appendChild(meta);
    container.className = "unity-mobile";
    canvas.className = "unity-mobile";
}

loadingBar.style.display = "block";

function showLoadingMessages() {
    let messages = [
        "Chargement des bureaux de l’entreprise…",
        "Préparation de Catherine, personnage atteint d'endométriose…",
        "Déploiement des collègues et du manager de Catherine…",
        "Prise du rendez-vous avec la médecine du travail…",
        "Rédaction des emails entre collègues…",
        "Validation des contenus par le comité scientifique d’EndoFrance…"
    ];
    loadingText.innerHTML = messages[0];
    let i = 1;
    let timer = setInterval(() => {
        loadingText.innerHTML = messages[i];
        i = (i + 1) % messages.length;

        if (i === 0) {
            clearInterval(timer);
            setTimeout(() => {
                loadingText.innerHTML = "Le serious game peut prendre 1 à 2 minutes à charger, merci pour votre patience.";
            }, 5000);
            setTimeout(() => {
                loadingText.innerHTML = "Merci de patienter, le chargement est plus long que prévu en raison d'une faible connexion internet. Privilégiez une connexion Wifi.";
            }, 30000);
        }
    }, 5000);
}

function handleFullscreen(unityInstance) {
    document.documentElement.requestFullscreen();
    if (document.documentElement.requestFullscreen) {
        document.documentElement.requestFullscreen();
    } else if (document.documentElement.mozRequestFullScreen) {
        document.documentElement.mozRequestFullScreen();
    } else if (document.documentElement.webkitRequestFullscreen) {
        document.documentElement.webkitRequestFullscreen();
    } else if (document.documentElement.msRequestFullscreen) {
        document.documentElement.msRequestFullscreen();
    }

    if (screen.orientation && screen.orientation.lock) {
        screen.orientation.lock('landscape');
    } else if (screen.lockOrientation) {
        screen.lockOrientation('landscape');
    }
}

function isDevicePerformant() {
    const cpuCores = navigator.hardwareConcurrency; 
    const ramGB = navigator.deviceMemory; 
    return (cpuCores !== undefined ? cpuCores >= 4 : true) && (ramGB !== undefined ? ramGB >= 4 : true);
}

function waitForIOSValue() {
    return new Promise((resolve) => {
        function onMessage(event) {
            if (event.data.ios !== undefined) {
                resolve(event.data.ios);
                window.removeEventListener('message', onMessage); // Retirer l'écouteur après réception
            }
        }
        window.addEventListener('message', onMessage);
    });
}

async function runGame() {
    try {
        let isIOS = /iPhone|iPad|iPod/i.test(navigator.userAgent) || (navigator.userAgent.includes("Mac") && "ontouchend" in document);
        let isIOSIframe = false;

        if (window.top !== window.self) {
            try {
                isIOSIframe = (await waitForIOSValue()) === 'true';
            } catch (e) {
                console.warn("Impossible de récupérer la valeur iOS dans l'iframe:", e);
            }
        }

        if (!isDevicePerformant()) {
            loadingText.innerHTML = "Désolé, votre appareil n'est pas assez performant pour jouer à ce jeu.";
            return;
        } 
        // Assurez-vous que speedTestAsync est défini
        else if (typeof speedTestAsync === 'function' && (await speedTestAsync()).value < 100) {
            loadingText.innerHTML = "Désolé votre connexion internet n'est pas assez performante.";
            return;
        }

        if (isIOS && !isIOSIframe) {
            loadingText.innerHTML = "<a href=\"https://endofrance.org/la-maladie-endometriose/endotravail-mobile-ios/\" target=\"_blank\" style=\"color: white;\">Votre version d'iOS ne supporte pas le plein écran.</a>";
            return;
        }

        showLoadingMessages();

        var script = document.createElement("script");
        script.src = loaderUrl;
        script.onload = () => {
            createUnityInstance(canvas, config, (progress) => {
                progressBarFull.style.width = 100 * progress + "%";
            }).then((unityInstance) => {
                loadingBar.style.display = "none";
                fullscreenButton.classList.remove("disabled");
                // Vous pouvez gérer le plein écran ici.
            }).catch((message) => {
                alert(message);
            });
        };
        document.body.appendChild(script);
    } catch (error) {
        loadingText.innerHTML = "Une erreur est survenue.";
        console.error("Erreur dans runGame:", error);
    }
}


runGame();
