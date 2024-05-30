$(document).ready(function () {
    // Load All Applicants
    GetAllWaitingApplicants()
    // Load All Remporary Reject Applicants
    GetAllTempRejectApplicants()
    // Display the Queue Number
    DisplayCurrentServe();

    // Click Event for the Next button
    $('#nextBtn').on("click", GetNextQueueNumber);
    // Click Event for the Passed button
    $('#passedBtn').on("click", PassedqueueNumber);
    // Click Event for the Failed button
    $('#failedBtn').on("click", FailedqueueNumber);
    // Click Event for the Call button
    $('#callBtn').on("click", CallQueueNumber);
    // Click Event for the Temporary Reject applicant button
    $('#tempRejectBtn').on("click", TempRejectQueueNumber);
    // Click Event for the Temporary Reject applicant Serve button
    $(document).on("click", '.tempServeBtn', TempServeQueueNumber);
    // Click Event for the Temporary Reject applicant Cancel button
    $(document).on("click", '.rejectBtn', RejectQueueNumber);

});

const displayDateTime = () => {
    var currentTime = new Date();
    var options = { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true };
    var formattedTime = currentTime.toLocaleString('en-US', options);
    var optionsDate = { year: 'numeric', month: 'long', day: 'numeric' };
    var formattedDate = currentTime.toLocaleString('en-US', optionsDate);

    $('#time').text(formattedTime);
    $('#date').text(formattedDate);
};
// Initial display
displayDateTime();
setInterval(displayDateTime, 1000);


//Function to Call the queue number
function CallQueueNumber() {
    var button = $(this);
    $.ajax({
        type: 'GET',
        url: "/HR/QueueCallDisplay",
        dataType: "json",
        success: function (result) {
            if (result != null || result != undefined) {
                const applicant = result.queue;
                servingDisplay.innerText = " A - " + applicant.applicantId

                servingDisplay.classList.add("blink-red");
                button.prop('disabled', true);

                setTimeout(function () {
                    servingDisplay.classList.remove("blink-red");
                }, 3000);

                setTimeout(function () {
                    button.prop('disabled', false);
                }, 2500);


            } else {
                alert("There is no Queue Number.");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

//Function to pass the queue number
function FailedqueueNumber() {
    var button = $(this);

    $.ajax({
        type: 'GET',
        url: "/HR/FailedQueue",
        dataType: "json",
        success: function (result) {
            console.log(result);
            if (result != null || result != undefined) {

                //Display the Waiting Applicants
                GetAllWaitingApplicants();
                //Display the Queue Number
                DisplayCurrentServe();

                button.prop('disabled', true);
                setTimeout(function () {
                    button.prop('disabled', false);
                }, 1000);

            } else {
                alert("There is no Queue number");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

//Function to pass the queue number
function PassedqueueNumber() {
    var button = $(this);

    $.ajax({
        type: 'GET',
        url: "/HR/PassedQueue",
        dataType: "json",
        success: function (result) {
            console.log(result);
            if (result != null || result != undefined) {

                //Display the Waiting Applicants
                GetAllWaitingApplicants();
                //Display the Queue Number
                DisplayCurrentServe();

                button.prop('disabled', true);
                setTimeout(function () {
                    button.prop('disabled', false);
                }, 1000);

            } else {
                alert("There is no Queue number");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

// Function to get the next queue number
function GetNextQueueNumber() {
    var button = $(this);

    $.ajax({
        type: 'GET',
        url: "/HR/NextQueue",
        dataType: "json",
        success: function (result) {
            if (result && !result.error) {
                // Display the Waiting Applicants
                GetAllWaitingApplicants();
                // Display the Queue Number
                DisplayCurrentServe();

                button.prop('disabled', true);
                setTimeout(function () {
                    button.prop('disabled', false);
                }, 1000);
            } else if (result && result.error) {
                alert(result.error);
            } else {
                alert("There is no Queue Number.");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

//Load All Applicants
function GetAllWaitingApplicants() {
    $.ajax({
        type: "get",
        url: "/HR/GetAllWaitingApplicants",
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
            if (result == null || result == undefined || result.length <= 0) {
                var object = '<tr><td class="text-center" colspan="6">There is no waiting applicant yet.</td></tr>';
                $('#tableBody').html(object);
            } else {
                var object = '';
                $.each(result, function (index, item) {
                    var applicant = item.applicant;

                    object += '<tr>';
                    object += '<th> A-' + applicant.applicantId + '</th>';
                    object += '<td>' + applicant.name + '</td>';
                    object += '<td>' + applicant.address + '</td>';
                    object += '<td>' + applicant.email + '</td>';
                    object += '<td>' + applicant.contactNumber + '</td>';
                    object += '<td>' + applicant.position + '</td>';
                    object += '</tr>';
                });

                $('#tableBody').html(object);
            }
        },
        error: function () {
            console.log('Unable to Fetch the Data.');
        }

    });
}

//Display Current Serve
function DisplayCurrentServe() {
    var servingDisplay = document.getElementById("servingDisplay");
    $.ajax({
        type: 'GET',
        url: "/HR/QueueServeDisplay",
        dataType: "json",
        success: function (result) {
            if (result != null || result != undefined) {
                servingDisplay.innerText = " A - " + result.applicantId
            } else {
                servingDisplay.innerText = "----";
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

function TempRejectQueueNumber() {
    var button = $(this);

    $.ajax({
        type: 'GET',
        url: "/HR/TempRejectQueue",
        dataType: "json",
        success: function (result) {
            if (result != null || result != undefined) {

                //Get All Temporary Reject Applicants
                GetAllTempRejectApplicants();

                //Display the Queue Number
                DisplayCurrentServe();

                button.prop('disabled', true);
                setTimeout(function () {
                    button.prop('disabled', false);
                }, 1000);

            } else {
                alert("There is no Queue number");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}
function GetAllTempRejectApplicants() {
    $.ajax({
        type: "get",
        url: "/HR/GetAllTempRejectApplicants",
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
            if (!result || result.length === 0) {
                var object = '<tr><td class="text-center" colspan="7">There are no temporary rejected applicants yet.</td></tr>';
                $('#tempRejectBody').html(object);
            } else {
                result.sort((a, b) => new Date(a.temporaryRejected_At) - new Date(b.temporaryRejected_At));

                var object = '';
                result.forEach(function (item) {
                    var applicant = item.applicant;
                    var rejectedAt = new Date(item.temporaryRejected_At);
                    var formattedDateTime = formatDate(rejectedAt);

                    var serveBtn = `<button class="btn btn-sm btn-success tempServeBtn" id="${applicant.applicantId}">Serve</button>`;
                    var rejectBtn = `<button class="btn btn-sm btn-secondary rejectBtn" id="${applicant.applicantId}">Reject</button>`;
                    var btns = `<div class="d-flex flex-column flex-lg-row gap-2 justify-content-center">${serveBtn}${rejectBtn}</div>`;

                    object += '<tr>';
                    object += '<th>';
                    object += btns;
                    object += `<small class="mt-2" style="font-size: 12px;">${formattedDateTime}</small>`;
                    object += '</th>';
                    object += `<th>A-${applicant.applicantId}</th>`;
                    object += `<td>${applicant.name}</td>`;
                    object += `<td>${applicant.address}</td>`;
                    object += `<td>${applicant.email}</td>`;
                    object += `<td>${applicant.contactNumber}</td>`;
                    object += `<td>${applicant.position}</td>`;
                    object += '</tr>';
                });

                $('#tempRejectBody').html(object);
            }
        },
        error: function () {
            console.log('Unable to fetch the data.');
        }
    });
}

function formatDate(date) {
    var options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' };
    return date.toLocaleString('en-US', options).toUpperCase();
}

//SERVED the Temporary Reject
function TempServeQueueNumber() {
    var appId = $(this).attr('id');

    $.ajax({
        type: 'GET',
        url: "/HR/ServeTempRejectQueue",
        data: { id: appId },
        dataType: "json",
        success: function (result) {
            if (result && !result.error) {
                //Display the Queue Number
                DisplayCurrentServe();
                //Get All Temporary Reject Applicants
                GetAllTempRejectApplicants();
              
            } else if (result && result.error) {
                alert(result.error);
            } else {
                alert("There is no Queue Number.");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

//Reject the Temporary Reject
function RejectQueueNumber() {
    var button = $(this).attr('id');

    $.ajax({
        type: 'GET',
        url: "/HR/RejectQueue",
        data: { id: button},
        dataType: "json",
        success: function (result) {
            if (result != null || result != undefined) {

                //Get All Temporary Reject Applicants
                GetAllTempRejectApplicants();

                button.prop('disabled', true);
                setTimeout(function () {
                    button.prop('disabled', false);
                }, 1000);

            } else {
                alert("There is no Queue number");
            }
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}