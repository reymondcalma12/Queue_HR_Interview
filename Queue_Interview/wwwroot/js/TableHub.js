//Create Connection
var connectionQueueHub = new signalR.HubConnectionBuilder().withUrl("/hubs/tableHub").build();
function LoadStages() {
    var response = JSON.parse(localStorage.getItem("stageTable"));

    if (response != null && response != undefined && response.length > 0) {
        $('#StageId').attr('disabled', false);
        $('#TableId').html('<option>--select table--</option>');

        $.each(response, function (i, data) {
            $('#StageId').append('<option value=' + data.stageId + '>' + data.stageName + '</option>');
        });
    } else {
        $('#StageId').attr('disabled', true);
        $('#TableId').attr('disabled', true);
        $('#StageId').html('<option>--stage not available--</option>');
        $('#TableId').html('<option>--table not available--</option>');
    }

}

function LoadTables(stageId) {
    $('#TableId').empty();
    $('#TableId').attr('disabled', true);
    var result = JSON.parse(localStorage.getItem("stageTable"));
    var response = result.find(x => x.stageId == stageId).tables;

    if (response != null && response != undefined && response.length > 0) {
        $('#TableId').attr('disabled', false);

        if (stageId == 3) {
            $('#TableId').append('<option selected disabled>--select room--</option>');
        } else {
            $('#TableId').append('<option selected disabled>--select table--</option>');
        }

        $.each(response, function (i, data) {
            $('#TableId').append('<option value=' + data.tableId + '>' + data.username + '</option>');
        });
    } else {
        $('#TableId').attr('disabled', true);
        $('#TableId').html('<option>--Table not available--</option>');
    }

}

connectionQueueHub.on("GetOfflineTables", function (response) {
    localStorage.setItem("stageTable", JSON.stringify(response));
    if ($('#StageId').length && $('#StageId').val().length) {
        LoadTables($('#StageId').val());
    }
})

//connectionQueueHub.on("ActiveUsers", function (response) {
//    console.log("ActiveUsers", response)
//})

//connectionQueueHub.on("UserIsOnline", function (response) {
//    console.log("OnlineTable: ", response)
//})

//connectionQueueHub.on("UsersIsOffline", function (response) {
//    console.log("OfflineTable", response)
//})

function fulfilled() {
    console.log("Table Connection Successful");
}

function rejected() {
    console.log("Table Connection failed");
}



// Start connection
connectionQueueHub.start().then(fulfilled, rejected);

