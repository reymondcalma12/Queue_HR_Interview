// Create Connection
var connectionQueueHub = new signalR.HubConnectionBuilder().withUrl("/hubs/queueHub").build();


function speak(text) {
    const synth = window.speechSynthesis;
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.rate = 1.0;
    utterance.pitch = 1.0;
    synth.speak(utterance);
}

function playBackgroundMusic() {
    const audio = new Audio('/Audio/ascend.mp3');

    audio.addEventListener('canplaythrough', () => {
        audio.play().catch((error) => {
            console.error('Error playing audio:', error);
        });
    });
}
//Modal to Interact the Page
document.addEventListener('DOMContentLoaded', function () {
    var overlay = document.createElement('div');
    overlay.style.cssText =
        'position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5); display: none; justify-content: center; align-items: center; z-index: 9999; text-align: center;';

    var interactionContent = document.createElement('div');
    interactionContent.style.cssText =
        'background-color: #fff; padding: 20px; border-radius: 5px;';

    var interactionText = document.createElement('p');
    interactionText.textContent = 'Please Click the button to continue.';
    interactionText.style.fontSize = '20px';
    interactionText.style.marginBottom = '10px';

    var interactionButton = document.createElement('button');
    interactionButton.textContent = 'Continue';
    interactionButton.style.cssText =
        'padding: 10px 20px; background-color: #007bff; color: #fff; border: none; border-radius: 5px; cursor: pointer;';

    interactionButton.addEventListener('click', function () {
        overlay.style.display = 'none';
    });

    interactionContent.appendChild(interactionText);
    interactionContent.appendChild(interactionButton);
    overlay.appendChild(interactionContent);
    document.body.appendChild(overlay);

    overlay.style.display = 'flex';
});

// Call QueueNumber in Monitor
connectionQueueHub.on("CallQueueApplicant", function (value, tableName, stageName) {
    var tableId = "table-" + value.tableId;
    var servingDisplay = document.getElementById(tableId);

    if (value != null || value != undefined) {
        servingDisplay.innerText = " A - " + value.applicantId;
        servingDisplay.classList.add("blink-red");

        playBackgroundMusic();

        setTimeout(function () {
            var appid = "A-" + value.applicantId;
            var speechText = appid + " Please proceed to " + tableName + " AT " + stageName;
            speak(speechText);
        }, 500);

        setTimeout(function () {
            servingDisplay.classList.remove("blink-red");
        }, 3000);
    } else {
        servingDisplay.innerText = "----";
    }
});




// Display QueueNumber in Monitor
connectionQueueHub.on("MonitorDisplayQueue", function (value) {
    var tableId = "table-" + value.tableId;
    var servingDisplay = document.getElementById(tableId);
    if (value != null || value != undefined) {
        servingDisplay.innerText = " A - " + value.applicantId;
    } else {
        servingDisplay.innerText = "----";
    }
});

// Remove QueueNumber in Monitor
connectionQueueHub.on("RemoveQueueMonitor", function (value) {
    var tableId = "table-" + value;
    var servingDisplay = document.getElementById(tableId);
    servingDisplay.innerText = "----";
});


function fulfilled() {
    console.log("Connection Successful");
}

function rejected() {
    console.log("Connection failed");
}
// Start connection
connectionQueueHub.start().then(fulfilled, rejected);