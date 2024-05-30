//Create Connection
var connectionQueueHub = new signalR.HubConnectionBuilder().withUrl("/hubs/queueHub").build();

//Update every Waiting applicant on each Steps/Stage
connectionQueueHub.on("UpdateApplicantQueue", function () {
    GetAllWaitingApplicants();
});


////From The Application Form into the HR Table
connectionQueueHub.on("UpdateWaitingStage1", function () {
    GetAllWaitingApplicants();
});

//Update every TEMPORARY Rejected applicant tables
connectionQueueHub.on("UpdateApplicantTempReject", function () {
    GetAllTempRejectApplicants();
});

// Display QueueNumber in Serving Menu of that user
connectionQueueHub.on("DisplayQueue", function () {
    DisplayCurrentServe()
});



function fulfilled() {
    console.log("Connection Successful");
}

function rejected() {
    console.log("Connection failed");
}
// Start connection
connectionQueueHub.start().then(fulfilled, rejected);